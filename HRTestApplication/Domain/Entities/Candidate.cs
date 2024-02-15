using HRTestApplication.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRTestApplication.Domain.Entities;
public class Candidate : Person
{
    public enum CandidateSteps
    {
        Reject = -1,
        First,
        Second,
        Task,
        Probation,
        Success,
    }

    public CandidateSteps CurrentStep { get; set; } = CandidateSteps.First;

    public bool IsTaskRequired { get; set; }

    public int VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
}
