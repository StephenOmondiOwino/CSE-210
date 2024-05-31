using System;
using System.Collections.Generic;

// Comment class
class Comment
{
    public string Author { get; set; }
    public string Text { get; set; }

    public Comment(string author, string text)
    {
        Author = author;
        Text = text;
    }
}

// Video class
class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; }
    private List<Comment> comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return comments.Count;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {Length} seconds");
        Console.WriteLine($"Number of comments: {GetCommentCount()}");
        Console.WriteLine("Comments:");
        foreach (var comment in comments)
        {
            Console.WriteLine($"{comment.Author}: {comment.Text}");
        }
        Console.WriteLine();
    }
}

// Main program
class Program
{
    static void Main(string[] args)
    {
        // Create videos
        Video video1 = new Video("Understanding Abstraction", "John Doe", 300);
        Video video2 = new Video("Learning Python", "Jane Smith", 420);
        Video video3 = new Video("Advanced OOP Concepts", "Emily Davis", 360);

        // Add comments to videos
        video1.AddComment(new Comment("Alice", "Great video!"));
        video1.AddComment(new Comment("Bob", "Very informative."));
        video1.AddComment(new Comment("Charlie", "Helped me a lot, thanks!"));

        video2.AddComment(new Comment("David", "Excellent tutorial."));
        video2.AddComment(new Comment("Eve", "Clear and concise."));
        video2.AddComment(new Comment("Frank", "Loved it!"));

        video3.AddComment(new Comment("Grace", "Very useful information."));
        video3.AddComment(new Comment("Heidi", "Nice explanations."));
        video3.AddComment(new Comment("Ivan", "Really well made video."));

        // Store videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display video information
        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}
