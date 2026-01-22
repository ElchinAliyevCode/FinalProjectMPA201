namespace FinalProjectMPA201.Helpers;

public static class FileHelper
{
    public static bool CheckSize(this IFormFile file, int mb)
    {
        return file.Length < mb * 1024 * 1024;
    }

    public static bool CheckType(this IFormFile file, string type)
    {
        return file.ContentType.Contains(type);
    }

    public static async Task<string> UploadFileAsync(this IFormFile file,string folderPath)
    {
        var uniqueName = Guid.NewGuid().ToString() + file.FileName;

        var path = Path.Combine(folderPath, uniqueName);

        using FileStream stream = new FileStream(path, FileMode.Create);

        await file.CopyToAsync(stream);

        return uniqueName;
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }


}
