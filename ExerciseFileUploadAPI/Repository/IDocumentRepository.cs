using SharedProjects;
using System.Collections.Generic;

namespace ExerciseFileUploadAPI.Repository
{
    public interface IDocumentRepository
    {
        bool UploadDocument(MyDocuments myDocuments);
        List<MyDocuments> GetList();
    }
}
