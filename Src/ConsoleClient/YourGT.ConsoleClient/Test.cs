using YourGT.DataAccess.EFCore;
using YourGT.DataAccess.EFCore.Repositories;
using YourGT.Shared.Entities;

namespace YourGT.ConsoleClient;

public static class Test
{
    public static void Education(YourGTDbContext context)
    {
        var educationRepository = new EducationRepository(context);
        var subjectRepository = new SubjectRepository(context);

        //var education = new Education();
        //education.Id = 1;
        //education.Name = "testEeducation1";
        //education.Description = "testDescription";
        //education.Completed = false;
        //education.Semesters = 4;

        var subject = new Subject();
        subject.Name = "TestSubject2";
        subject.Description = "testDescription2";
        subject.Education = educationRepository.GetById(2);

        subjectRepository.Add(subject);

        //educationRepository.Update(1, education);

        //var educations = educationRepository.GetAll();

        var subjects = subjectRepository.GetAll();
        foreach (var s in subjects)
        {
            Console.WriteLine($"Subject: {s.Id}");
            Console.WriteLine($"Name: {s.Name}");
            Console.WriteLine($"Description: {s.Description}");
        }
    }
}