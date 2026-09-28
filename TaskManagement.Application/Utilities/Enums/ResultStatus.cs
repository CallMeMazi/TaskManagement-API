using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.Utilities.Enums;
public enum ResultStatus
{
    [Display(Name = "موفق.")]
    OK = 200,
    [Display(Name = "درخواست نامعتبر!")]
    BadRequest = 400,
    [Display(Name = "یافت نشد!")]
    NotFound = 404,
    [Display(Name = "وضعیت نامعتبر!")]
    Conflict = 409,
    [Display(Name = "اخراز هویت نشده!")]
    Unauthorized = 401,
    [Display(Name = "عدم دسترسی!")]
    Forbidden = 403
}
