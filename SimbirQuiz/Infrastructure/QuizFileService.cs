using SimbirQuiz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirQuiz.Infrastructure
{
    class QuizFileService
    {
        public Quiz Open()
        {
            FileOpen open = new FileOpen();
            if (open.ShowDialog() == true)
            {
                if (open.SelectedQuizPath != null)
                {
                    JsonFileService jsonFile = new JsonFileService();
                    return jsonFile.Open(open.SelectedQuizPath);
                }
            }

            return null;
        }

        public void Save(Quiz _savingQuiz)
        {
            string savePathQuiz = @"Quizzes\" + _savingQuiz.Name;
            Directory.CreateDirectory(savePathQuiz);
            foreach (var task in _savingQuiz.Tasks)
            {
                string pathTask = "";
                if (task.Images.Count > 0 || task.Media != null)
                {
                    pathTask = savePathQuiz + "\\Task" + _savingQuiz.Tasks.IndexOf(task); 
                    Directory.CreateDirectory(pathTask);
                }

                if (task.Images.Count > 0)
                {
                    ObservableCollection<string> newPaths = new ObservableCollection<string>();
                    string pathTaskImages = pathTask + "\\Images";
                    Directory.CreateDirectory(pathTaskImages);
                    foreach (var image in task.Images)
                    {
                        if (File.Exists(image))
                        {                            
                            string savePathImage = pathTaskImages + "\\" + Path.GetFileName(image);
                            int count = 1;
                            while(File.Exists(Path.GetFullPath(savePathImage)))
                            {
                                savePathImage = pathTaskImages + "\\" + Path.GetFileNameWithoutExtension(image) + "(" + count + ")" + Path.GetExtension(image);
                                count++;
                            }

                            File.Copy(image, Path.GetFullPath(savePathImage), true);
                            newPaths.Add(savePathImage);                                                   
                        }
                    }
                    task.Images = newPaths;
                }

                if (task.Media != null)
                {
                    if (File.Exists(task.Media))
                    {                        
                        string savePathMedia = pathTask + "\\" + Path.GetFileName(task.Media);
                        File.Copy(task.Media, Path.GetFullPath(savePathMedia), true);
                        task.Media = savePathMedia;
                    }
                }
            }
            JsonFileService jsonFile = new JsonFileService();
            jsonFile.Save(Path.GetFullPath(savePathQuiz + "\\" + _savingQuiz.Name + ".json"), _savingQuiz);
        }
    }
}
