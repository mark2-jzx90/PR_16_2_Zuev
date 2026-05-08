using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PR_16_2_2var
{
    public class CsvService
    {
        // Сохранение данных в CSV
        public static void SaveToCsv(string filePath, string speciality, List<SubjectGrade> subjects)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.WriteLine("Специальность,Предмет,Оценка");
                foreach (var item in subjects)
                {
                    sw.WriteLine($"{speciality},{item.Предмет},{item.Оценки}");
                }
            }
        }

        // Загрузка данных из CSV
        public static (string speciality, List<SubjectGrade> subjects) LoadFromCsv(string filePath)
        {
            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 2)
                throw new Exception("Файл пуст");
            var newList = new List<SubjectGrade>();
            string speciality = "";
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 3)
                {
                    speciality = parts[0];
                    string subject = parts[1];
                    if (int.TryParse(parts[2], out int grade))
                    {
                        newList.Add(new SubjectGrade { Предмет = subject, Оценки = grade });
                    }
                }
            }
            return (speciality, newList);
        }
    }
}
