using GradeMaster.Shared.Core.Entities;
using GradeMaster.Shared.Core.Interfaces;

namespace GradeMaster.DataAccess.Core.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly GradeMasterDbContext _context;

    public SubjectRepository(GradeMasterDbContext context)
    {
        _context = context;
    }


    public Subject? GetById(int id)
    {
        return _context.Subjects.Find(id);
    }

    public List<Subject> GetAll()
    {
        return _context.Subjects.ToList();
    }

    public Subject Add(Subject subject)
    {
        // maybe include if statement
        _context.Educations.Attach(subject.Education);
        _context.Grades.AttachRange(subject.Grades);

        _context.Subjects.Add(subject);

        _context.SaveChanges();

        return subject;
    }

    public void Update(int id, Subject subject)
    {
        _context.Educations.Attach(subject.Education);
        _context.Grades.AttachRange(subject.Grades);

        _context.Subjects.Update(subject);

        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var existingSubject = GetById(id);

        if (existingSubject != null)
        {
            // the deletion of the related entities is handled by the database
            _context.Subjects.Remove(existingSubject);
            _context.SaveChanges();
        }
    }
}