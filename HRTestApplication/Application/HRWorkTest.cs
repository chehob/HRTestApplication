using HRTestApplication.Application.Common.Interfaces;
using HRTestApplication.Controllers;
using HRTestApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRTestApplication.Application;

public class HRWorkTest : IHRWork
{        
    private readonly ILogger<HomeController> _logger;
    private readonly IApplicationDbContext _context;

    private const int rejectionRate = 20;
    private const int advanceRate = 40;

    public HRWorkTest(ILogger<HomeController> logger, IApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task DoHRWork()
    {
        // HR специалист отдает приоритет вакансиям с наибольшим сроком активности, поэтому покажем их первыми в списке
        var query = from division in _context.Divisions
                    join vacancy in _context.Vacancies on division.Id equals vacancy.DivisionId
                    where vacancy.IsClosed == false
                    orderby vacancy.StepsActive descending
                    select new
                    {
                        division.Name,
                        vacancy
                    };

        var firstRecord = await query.FirstOrDefaultAsync();
        if (firstRecord == null)
        {
            _logger.LogWarning("No active vacancies!");
            return;
        }

        // поиск кандидатов, подходящих на вакансию
        await SearchCandidates(firstRecord.vacancy);        

        // Симуляция прохождения этапов приема на работу - в случайном порядке некоторые кандидаты получают отказ, некоторые проходят на следующий этап.
        await SimulateStep();
    }

    private async Task SearchCandidates(Vacancy vacancy)
    {
        var results = await SearchCandidatesOnWeb(vacancy.Description);

        foreach (var result in results)
        {
            AddCandidate(result.Name, result.IsTaskRequired, vacancy);
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }

    private void AddCandidate(string name, bool isTaskRequired, Vacancy vacancy)
    {
        Candidate candidate = new()
        {
            Name = name,
            Vacancy = vacancy,
            IsTaskRequired = isTaskRequired
        };

        _context.Candidates.Add(candidate);
    }

    // Данные кандидата из поискового запроса
    class SearchResult
    {
        public string Name { get; set; }
        public bool IsTaskRequired { get; set; } = false;
    }

    // Заглушка. Симуляция поиска кандидата на внешних ресурсах
    private async Task<IEnumerable<SearchResult>> SearchCandidatesOnWeb(string parameters)
    {
        await Task.CompletedTask;

        return [new() { Name = "Василий" }, new() { Name = "Антон", IsTaskRequired = true }];
    }

    private async Task SimulateStep()
    {
        Random r = new();

        foreach (var vacancy in _context.Vacancies.Include(v => v.Candidates).Where(v => v.IsClosed == false))
        {
            vacancy.StepsActive++;

            foreach (var candidate in vacancy.Candidates.Where(c => c.CurrentStep != Candidate.CandidateSteps.Reject))
            {
                int roll = r.Next(0, 100) + 1;

                if (roll <= rejectionRate)
                    RejectCandidate(candidate);
                else if (roll >= 100 - advanceRate)
                {
                    AdvanceCandidate(candidate);
                    if (candidate.CurrentStep == Candidate.CandidateSteps.Success)
                    {
                        CloseVacancy(vacancy);
                        break;
                    }
                }
            }
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }

    private void RejectCandidate(Candidate candidate)
    {
        if (candidate.CurrentStep == Candidate.CandidateSteps.Reject)
            return;

        candidate.CurrentStep = Candidate.CandidateSteps.Reject;
    }

    // Кандидат проходит первый, второй и, если требуется, третий (тестовое задание) этапы приема на работу и наконец проходит испытательный срок
    private void AdvanceCandidate(Candidate candidate)
    {
        switch (candidate.CurrentStep)
        {
            case Candidate.CandidateSteps.Reject:
                _logger.LogWarning("Cannot advance rejected candidate!");
                return;
            case Candidate.CandidateSteps.First:
                candidate.CurrentStep = Candidate.CandidateSteps.Second;
                break;
            case Candidate.CandidateSteps.Second:
                if (candidate.IsTaskRequired)
                {
                    candidate.CurrentStep = Candidate.CandidateSteps.Task;
                }
                else
                {
                    candidate.CurrentStep = Candidate.CandidateSteps.Probation;
                }
                break;
            case Candidate.CandidateSteps.Task:
                candidate.CurrentStep = Candidate.CandidateSteps.Probation;
                break;
            case Candidate.CandidateSteps.Probation:
                candidate.CurrentStep = Candidate.CandidateSteps.Success;
                break;
        }
    }

    private void CloseVacancy(Vacancy vacancy)
    {
        vacancy.IsClosed = true;
    }
}
