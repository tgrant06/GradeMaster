using Microsoft.EntityFrameworkCore;
using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;

namespace GradeMaster.DataAccess.Core.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly GradeMasterDbContext _context;

    public GradeRepository(GradeMasterDbContext context)
    {
        _context = context;
    }

    public Grade? GetById(int id)
    {
        return _context.Grades.Find(id);
    }

    public List<Grade> GetAll()
    {
        return _context.Grades.ToList(); // add as no tracking maybe
    }

    public Grade Add(Grade grade)
    {
        _context.Subjects.Attach(grade.Subject);
        _context.Weights.Attach(grade.Weight);

        _context.Grades.Add(grade);

        _context.SaveChanges();

        return grade;
    }

    public void Update(int id, Grade grade)
    {
        _context.Subjects.Attach(grade.Subject);
        _context.Weights.Attach(grade.Weight);

        _context.Grades.Update(grade);

        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var existingGrade = GetById(id);

        if (existingGrade != null)
        {
            _context.Grades.Remove(existingGrade);
            _context.SaveChanges();
        }
    }
}