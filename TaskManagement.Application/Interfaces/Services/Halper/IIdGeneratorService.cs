using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Interfaces.Services.Halper;

public interface IIdGeneratorService
{
    // Generates the next id for the given domain entity type
    GeneralResult<long> NextId(EntityType entityType);
    // Generates the next id by CLR type
    GeneralResult<long> NextId(Type entityClrType);
    // Reads the entity type tag encoded in the id
    GeneralResult<EntityType> GetEntityType(long id);
    // Splits an id into its bit-field parts
    GeneralResult<EntityIdPartsInternalDto> Decode(long id);
    // Validate id type
    GeneralResult ValidateTypeId(long id, EntityType entityType);
}
