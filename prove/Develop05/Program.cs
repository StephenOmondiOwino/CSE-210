using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

abstract class Goal
{
    public string Name { get; set; }
    public int Points { get; set; }

    public Goal(string name, int points)
    {
        Name = name;
        Points = points;
    }

    public abstract int RecordEvent();
    public abstract bool IsComplete();
    public abstract override string ToString();
}

class SimpleGoal : Goal
{
    public bool Completed { get; set; }

    public SimpleGoal(string name, int points) : base(name, points)
    {
        Completed = false;
    }

    public override int RecordEvent()
    {
        if (!Completed)
        {
            Completed = true;
            return Points;
        }
        return 0;
    }

    public override bool IsComplete()
    {
        return Completed;
    }

    public override string ToString()
    {
        string status = Completed ? "[X]" : "[ ]";
        return $"{status} Simple Goal: {Name} - {Points} points";
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name, points)
    {
    }

    public override int RecordEvent()
    {
        return Points;
    }

    public override bool IsComplete()
    {
        return false;
    }

    public override string ToString()
    {
        return $"[ ] Eternal Goal: {Name} - {Points} points";
    }
}

class ChecklistGoal : Goal
{
    public int TargetCount { get; set; }
    public int BonusPoints { get; set; }
    public int CurrentCount { get; set; }

    public ChecklistGoal(string name, int points, int targetCount, int bonusPoints) : base(name, points)
    {
        TargetCount = targetCount;
        BonusPoints = bonusPoints;
        CurrentCount = 0;
    }

    public override int RecordEvent()
    {
        if (CurrentCount < TargetCount)
        {
            CurrentCount++;
            if (CurrentCount == TargetCount)
            {
                return Points + BonusPoints;
            }
            return Points;
        }
        return 0;
    }

    public override bool IsComplete()
    {
        return CurrentCount >= TargetCount;
    }

    public override string ToString()
    {
        string status = IsComplete() ? "[X]" : "[ ]";
        return $"{status} Checklist Goal: {Name} - {Points} points each, {CurrentCount}/{TargetCount} completed, {BonusPoints} bonus points";
    }
}

class QuestTracker
{
    public List<Goal> Goals { get; set; }
    public int Score { get; set; }

    public QuestTracker()
    {
        Goals = new List<Goal>();
        Score = 0;
    }

    public void AddGoal(Goal goal)
    {
        Goals.Add(goal);
    }

    public void RecordEvent(string goalName)
    {
        Goal goal = Goals.FirstOrDefault(g => g.Name == goalName);
        if (goal != null)
        {
            Score += goal.RecordEvent();
        }
    }

    public void DisplayGoals()
    {
        foreach (Goal goal in Goals)
        {
            Console.WriteLine(goal);
        }
    }

    public void DisplayScore()
    {
        Console.WriteLine($"Current score: {Score}");
    }

    public void Save(string filename)
    {
        var data = new
        {
            score = Score,
            goals = Goals.Select(g => new
            {
                type = g.GetType().Name,
                name = g.Name,
                points = g.Points,
                completed = g is SimpleGoal sg ? sg.Completed : (bool?)null,
                currentCount = g is ChecklistGoal cg ? cg.CurrentCount : (int?)null,
                targetCount = g is ChecklistGoal tg ? tg.TargetCount : (int?)null,
                bonusPoints = g is ChecklistGoal bg ? bg.BonusPoints : (int?)null
            }).ToList()
        };

        string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filename, jsonString);
    }

    public void Load(string filename)
    {
        string jsonString = File.ReadAllText(filename);
        var data = JsonSerializer.Deserialize<SaveData>(jsonString);

        Score = data.score;
        Goals = new List<Goal>();

        foreach (var goalData in data.goals)
        {
            Goal goal = null;
            switch (goalData.type)
            {
                case "SimpleGoal":
                    goal = new SimpleGoal(goalData.name, goalData.points) { Completed = goalData.completed.Value };
                    break;
                case "EternalGoal":
                    goal = new EternalGoal(goalData.name, goalData.points);
                    break;
                case "ChecklistGoal":
                    goal = new ChecklistGoal(goalData.name, goalData.points, goalData.targetCount.Value, goalData.bonusPoints.Value) { CurrentCount = goalData.currentCount.Value };
                    break;
            }
            Goals.Add(goal);
        }
    }

    private class SaveData
    {
        public int score { get; set; }
        public List<GoalData> goals { get; set; }
    }

    private class GoalData
    {
        public string type { get; set; }
        public string name { get; set; }
        public int points { get; set; }
        public bool? completed { get; set; }
        public int? currentCount { get; set; }
        public int? targetCount { get; set; }
        public int? bonusPoints { get; set; }
    }
}

// Example usage
class Program
{
    static void Main()
    {
        QuestTracker tracker = new QuestTracker();

        // Adding goals
        tracker.AddGoal(new SimpleGoal("Run a marathon", 1000));
        tracker.AddGoal(new EternalGoal("Read scriptures", 100));
        tracker.AddGoal(new ChecklistGoal("Attend temple", 50, 10, 500));

        // Display goals
        tracker.DisplayGoals();

        // Record events
        tracker.RecordEvent("Read scriptures");
        tracker.RecordEvent("Attend temple");
        tracker.RecordEvent("Attend temple");
        tracker.RecordEvent("Run a marathon");

        // Display updated goals and score
        tracker.DisplayGoals();
        tracker.DisplayScore();

        // Save and load
        tracker.Save("goals.json");
        tracker.Load("goals.json");

        // Display loaded data
        tracker.DisplayGoals();
        tracker.DisplayScore();
    }
}
