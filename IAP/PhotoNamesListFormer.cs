using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAP
{
    public class PhotoNamesListFormer
    {
        private string _directory;

        public PhotoNamesListFormer(string directory)
        {
            _directory = directory;
        }

        public void NamesFormer()
        {
            string folderPath = _directory;
            string[] files = Directory.GetFiles(folderPath);

            List<string> uniqueNames = new List<string>();

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                int lastUnderscoreIndex = fileName.LastIndexOf('_'); // Находим индекс последнего символа подчеркивания
                if (lastUnderscoreIndex >= 0)
                {
                    string nameWithoutSuffix = fileName.Substring(0, lastUnderscoreIndex); // Удаляем часть после последнего символа подчеркивания
                    uniqueNames.Add(nameWithoutSuffix);
                }
            }

            uniqueNames = uniqueNames.Distinct().ToList(); // Получаем уникальные имена

            File.WriteAllLines("profile_names.txt", uniqueNames); // Сохраняем имена в файл

            Console.WriteLine("Уникальные имена сохранены в файл profile_names.txt.");
        }
    }
}
