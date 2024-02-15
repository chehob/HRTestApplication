using HRTestApplication.Domain.Common;

namespace HRTestApplication.Domain.Entities;
public class Vacancy : BaseEntity
{
    public bool IsClosed { get; set; } = false;

    // Трекер как долго открыта вакансия
    public int StepsActive { get; set; } = 0;

    public ICollection<Candidate> Candidates { get; } = new List<Candidate>();

    public int DivisionId { get; set; }
    public Division Division { get; set; }

    public string Description { get; set; }
}
