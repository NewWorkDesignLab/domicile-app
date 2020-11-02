using System;

[Serializable]
public class Participation
{
  public int id;
  public int user_id;
  public int scenario_id;
  public DateTime created_at;
  public DateTime updated_at;
}

[Serializable]
public class ParticipationGroup
{
  public Participation[] participations;
}
