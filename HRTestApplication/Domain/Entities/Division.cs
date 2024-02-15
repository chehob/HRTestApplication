using HRTestApplication.Domain.Common;

namespace HRTestApplication.Domain.Entities;
public class Division : BaseEntity
{
    public string Name { get; set; } = "";

    public ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
