using System.IO;
using System.Runtime.Serialization.Json;

using SimbirQuiz.Models;

namespace SimbirQuiz.Infrastructure
{
    public class JsonFileService : IFileService
    {
        public Quiz Open(string filename)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Quiz));

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                return (Quiz)jsonSerializer.ReadObject(fs);
            }
        }

        public void Save(string filename, Quiz savingQuiz)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Quiz));

            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                jsonSerializer.WriteObject(fs, savingQuiz);

                //foreach (var task in savingQuiz.Tasks)
                //{
                //    if (task.Images.Count != 0)
                //    {
                //        foreach (var image in task.Images)
                //        {
                //            File.Copy(image, filename);
                //        }
                //    }
                //    if (task.Videos.Count != 0)
                //    {
                //        foreach (var video in task.Videos)
                //        {
                //            File.Copy(video, filename);
                //        }
                //    }
                //    if (task.Audios.Count != 0)
                //    {
                //        foreach (var audio in task.Audios)
                //        {
                //            File.Copy(audio, filename);
                //        }
                //    }
                //}
            }
            

        }
    }
}

