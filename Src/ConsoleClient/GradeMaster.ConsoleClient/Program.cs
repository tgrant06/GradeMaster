using Microsoft.Extensions.Configuration;
using GradeMaster.DataAccess;
using GradeMaster.DataAccess.Repositories;
using GradeMaster.Common.Entities;

namespace GradeMaster.ConsoleClient;

internal class Program
{
    static void Main(string[] args)
    {
        var context = DbConnector.Connect();

        //put in a different file (above)

        Test.Education(context);

        //var education = new Education();
        //education.Name = "TestEducation1";
        //education.Description = "TestDescription1";
        //education.Completed = false;
        //education.Semesters = 4;

        //var educationRepository = new EducationRepository(context);

        //educationRepository.DeleteByIdAsync(1);

        //var education = educationRepository.GetByIdAsync(1);

        //education.Name = "TestEducation01";
        //education.Description = "TestDescription01";
        //education.Completed = true;
        //education.Semesters = 6;

        //educationRepository.UpdateAsync(1, education);

        //educationRepository.AddAsync(education);

        //var educations = educationRepository.GetAllAsync();

        //foreach (var e in educations)
        //{
        //    Console.WriteLine($"Education: {e.Id}");
        //    Console.WriteLine($"Name: {e.Name}");
        //    Console.WriteLine($"Description: {e.Description}");
        //    Console.WriteLine($"Semesters: {e.Semesters}");
        //    Console.WriteLine($"Completed: {e.Completed}");

        //}
        Console.WriteLine();
        Console.WriteLine("Hello, World!");
        Console.ReadLine();

        //var connectionString = config["ConnectionStrings:Default"];

        //Console.WriteLine(connectionStrings);
    }
}
