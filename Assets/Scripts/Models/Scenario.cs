using System;

[Serializable]
public class Scenario
{
  public int id;
  public int user_id;
  public DateTime created_at;
  public DateTime updated_at;
  public string name;
  public int number_rooms;
  public int time_limit;
  public int number_damages;
}

[Serializable]
public class ScenarioGroup
{
  public Scenario[] scenarios;
}
