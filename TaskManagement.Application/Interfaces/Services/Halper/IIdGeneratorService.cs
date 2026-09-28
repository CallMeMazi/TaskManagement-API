using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Interfaces.Services.Halper;

public interface IIdGeneratorService
{
    // Generates the next id for the given domain entity type
    long NextId(EntityType entityType);
    // Generates the next id by CLR type
    long NextId(Type entityClrType);
    // Reads the entity type tag encoded in the id
    EntityType GetEntityType(long id);
    // Splits an id into its bit-field parts
    EntityIdPartsInternalDto Decode(long id);
    // Validate id type
    bool ValidateTypeId(long id, EntityType entityType);
}
