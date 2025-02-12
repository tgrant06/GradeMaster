using GradeMaster.DataAccess;
using GradeMaster.DataAccess.Repositories;
using GradeMaster.Common.Entities;

namespace GradeMaster.ConsoleClient;

public static class Test
{
    public static void Education(GradeMasterDbContext context)
    {
        var educationRepository = new EducationRepository(context);
        var subjectRepository = new SubjectRepository(context);

        //var education = new Education();
        //education.Id = 1;
        //education.Name = "testEeducation1";
        //education.Description = "testDescription";
        //education.Completed = false;
        //education.Semesters = 4;

        //var subject = new Subject();
        //subject.Name = "TestSubject2";
        //subject.Description = "testDescription2";
        //subject.Education = educationRepository.GetByIdAsync(2);

        //subjectRepository.AddAsync(subject);

        //educationRepository.UpdateAsync(1, education);

        //var educations = educationRepository.GetAllAsync();

        var subjects = subjectRepository.GetAllAsync();
        foreach (var s in subjects.Result)
        {
            Console.WriteLine($"Subject: {s.Id}");
            Console.WriteLine($"Name: {s.Name}");
            Console.WriteLine($"Description: {s.Description}");
        }
    }
}