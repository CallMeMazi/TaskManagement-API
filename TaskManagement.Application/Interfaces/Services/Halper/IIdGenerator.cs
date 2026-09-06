using TaskManagement.Domain.Enums.Logs;

namespace TaskManagement.Application.Interfaces.Services.Halper;

public interface IIdGenerator
{
    // Generates the next id for the given domain entity type
    long NextId(EntityType entityType);
    // Generates the next id by CLR type
    long NextId(Type entityClrType);
    // Reads the entity type tag encoded in the id
    EntityType GetEntityType(long id);
    // Splits an id into its bit-field parts
    EntityIdParts Decode(long id);
}
