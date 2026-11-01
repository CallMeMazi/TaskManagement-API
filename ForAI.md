# CLAUDE.md

Project memory for Claude Code. Loaded at the start of every session. Keep new code consistent with the patterns below. Do not rename existing types to "fix" spelling unless the task is explicitly a rename.

## Commands

Run from the repository root.

```bash
dotnet restore TaskManagement.sln
dotnet build TaskManagement.sln
dotnet run --project TaskManagenemt.API/TaskManagenemt.API.csproj --launch-profile https
```

- `https` profile: `https://localhost:7097` and `http://localhost:5100`. Browser opens `/scalar/v1`. OpenAPI is `/openapi/v1.json`.
- `http` profile: `http://localhost:5100`.
- There is no test project and no `dotnet test` target. Do not claim tests passed.
- `appsettings.json`, `appsettings.Development.json`, and `TaskManagement-Test.http` are gitignored. The host needs both connection strings and the `AppSettings` section or options binding fails.
- EF Core packages are `10.0.12`. Match `dotnet-ef` to that version. Migrations project: `TaskManagement.Infrastructure`. Startup project: `TaskManagenemt.API`.

```bash
dotnet ef migrations add <Name> --project TaskManagement.Infrastructure --startup-project TaskManagenemt.API --context ApplicationDbContext --output-dir Persistence/Migrations/Application
dotnet ef database update --project TaskManagement.Infrastructure --startup-project TaskManagenemt.API --context ApplicationDbContext
```

Use `--context LogDbContext` and `--output-dir Persistence/Migrations/Logs` for the log database. Migration folders are gitignored. List existing migrations before creating an initial one.

## Architecture

`net10.0` solution. Reference direction:

`TaskManagenemt.API` → `TaskManagement.WebConfig` → `TaskManagement.Infrastructure` → `TaskManagement.Application` → `TaskManagement.Domain` → `TaskManagement.Common`

- Domain: entities, enums, domain services, repository contracts. No EF Core, no ASP.NET Core.
- Application: MediatR use cases, FluentValidation, AutoMapper, application services, pipelines.
- Infrastructure: EF Core, repositories, unit of work, JWT, password hashing, snowflake ids. It references Application so it can implement those ports.
- WebConfig: Problem Details handlers, folder spelled `ExceeptionHandlers`.
- API host (`Program.cs`): `AddControllers`, `AddOpenApi`, `MapScalarApiReference`, exception handler, `UseHttpsRedirection`, `UseAuthorization`, `MapControllers`. There are no controller classes. Do not document a route you did not add.

Composition methods: `RegisterAllApplicationLayerConfiguration()`, `RegisterAllInfrastructureLayerConfiguration(configuration)`, `RegisterExceptionHandlersAndServices()`, and `CompileAutoMapperConfiguration()` at startup (`CompileMappings` + `AssertConfigurationIsValid`). A new map that does not compile fails startup.

Pipeline order, outer to inner: `PerformanceBehavior`, `ValidationBehavior`, `TransactionBehavior`, handler. `PerformanceBehavior` has an empty block when elapsed time exceeds 1000 ms. `TransactionBehavior` is constrained to `ITransactionalCommand`. The only implementer is `CreateUserCommand`. The transaction is `IUnitOfWork` / `ApplicationDbContext`. `LogDbContext` is a different database and is not inside that transaction.

## Add a use case

`TaskManagement.Application/Features/{Area}/{Command|Query}/{Name}/`

- `{Name}Command` or `{Name}Query` as a `record` implementing `IRequest` or `IRequest<TResponse>`.
- `{Name}Handler : IRequestHandler<...>`.
- `{Name}Validator : AbstractValidator<TRequest>` when input has rules. `AddValidatorsFromAssemblyContaining<CreateUserValidator>()` picks it up.
- Validation and domain messages are Persian. Match that for user-facing text.
- Handlers call application services and `IUnitOfWork`. They do not construct a `DbContext`.
- Invariants go on the entity or a domain service. Persistence stays in Infrastructure.
- New DTO maps go under `MappingProfile/{Area}Profile/`. Startup asserts the whole configuration.
- DTOs live in `DTOs/RequestDTOs`, `DTOs/ResponseDTOs`, and `DTOs/InternalDTOs`.
- Pass `CancellationToken` through. Implement `ITransactionalCommand` only when the command must commit or roll back as one unit of work.

`GeneralResult` / `GeneralResult<T>` is the infrastructure helper result (`IsSuccess`, `Message`, `Result`). It is not the HTTP body. HTTP failures are Problem Details from the exception handlers.

## Domain facts

- Login id is the mobile number.
- Max 3 owned organizations (`OrganizationDomainService`). Max 3 active sessions (`UserTokenDomainService.EnsureCanLoginAsync`).
- Org roles: `Owner`, `Admin`, `Member`. Project roles: `Creator`, `Admin`, `Member`.
- Project status: `InProgress`, `Adjournment`, `Cancel`, `Finished`. Task status: `InProgress`, `Cancel`, `Dead`, `Finished`. Task type: `Single`, `Group`. Invitation status: `Pending`, `Accepted`, `Expired`, `Revoked`. Token status: `Active`, `Revoked`, `Expired`.
- `BaseEntity`: `long Id`, `IsDelete`, `CreatedAt`, `UpdatedAt`, `AssignId`, `SoftDelete`. Ids are snowflakes. Do not switch them to `Guid` or identity columns.
- `AssignId` throws if `Id != 0` or `id <= 0`. `CreatedAt` and other timestamps use `DateTime.Now`.
- Global query filter name is `IsDeleteFillter` (`!IsDelete`) on the application configurations.
- `IsRowVersion()` is mapped for `Project`, `Task`, and `OrganizationInvitation`.
- Delete behavior is restrict. Table names are pluralized (`Pluralize.NET`).
- `NormalizeStringInterceptor` runs only on `ApplicationDbContext` and rewrites Persian/Arabic string variants on Added and Modified string properties.
- Snowflake layout in `IdGeneratorService`: `[1 sign][42 timestamp][5 entityType][3 worker][10 sequence][3 version]`, version `1`, epoch `2026-01-01`. `WorkerId` must be 0–7. `EntityIdSaveChangesInterceptor` assigns an id when it is still 0. Not every entity type is in `EntityTypeMap` (log entities are not).
- `Organization.ValidateOrgCreating` checks capacity with `maxUser <= 5 && maxUser >= 50`, which cannot be true. Do not copy that condition.

## Auth

- `IPasswordService.Hash` / `Verify` wrap Identity `PasswordHasher`. Verify with `Verify` or `VerifyAndCheck`. Never compare two `Hash()` strings.
- Access token: HMAC-SHA256 plus `Aes128KW` / `Aes128CbcHmacSha256`. `JwtSetting.SecretKey` must be valid Base64. `JwtSetting.Encryptkey` (that spelling) is a UTF-8 16-byte AES key.
- Refresh token: 64 random bytes, Base64.
- `ValidateAccessTokenAndGetPrincipal` also checks the `deviceId` claim.
- `AuthService` stores and looks up access/refresh token hashes via `Password.Hash`. A salted hash cannot be found by hashing the same input again. Do not add another lookup of that shape. Use a deterministic digest for storage, or verify with the hasher API.
- No `AddAuthentication` and no `UseAuthentication` in `Program.cs`. `UseAuthorization()` alone does not authenticate a bearer token.

## HTTP errors

| Throw | Status |
|---|---|
| `ValidationFailureException`, `DomainValidationFailureException` | 400, `Errors` extension on the former |
| `UnAuthorizedException` | 401 |
| `ForbiddenException`, `DomainAuthorizationException` | 403 |
| `NotFoundException` | 404 |
| `ConflictException`, `DomainLogicalException` | 409 |
| any other exception | 500 from `GlobalExceptionHandler` |

Problem Details customization adds `isSuccess`, `traceId`, `timeStamp`. `AppException` and `DomainException` are excluded from exception-handler diagnostics.

## Spell these as they already exist

- `TaskManagenemt.API`
- `IAuthServiec`
- namespace `TaskManagement.Application.Interfaces.Services.Halper`
- `ExceeptionHandlers`
- `OrganozationInvitationConfiguration`
- `RegisterLogRepositpries`
- `IsDeleteFillter`
- `BaseCompliedQuery`, `GetByIdAsNoTrackingQueryg`
- `InvitationDominService`, `IInvitationDominService`, `ProjectDominService`, `IProjectDominService`
- `OrganizationMemberShip`, `ProjectMemberShip`
- `Encryptkey`
- `ValidateAcceessTokenQuery`

`IUserRepository` is registered twice in `RegisterAppRepositories`. Leave it unless you are editing that method.

## Defaults

- Nullable and implicit usings are on. Registration classes use C# `extension` blocks. Follow that when adding a method there.
- `Swashbuckle.AspNetCore` is referenced and unused. The document UI is Scalar. Do not wire Swagger unless asked.
- Do not add Redis, Docker, a test project, or a new project to the solution unless asked.
- Do not commit secrets. There is no `LICENSE` file; do not invent one.
