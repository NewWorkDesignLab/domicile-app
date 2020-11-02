using System;

[Serializable]
public class Execution
{
  public int id;
  public int participation_id;
  public DateTime created_at;
  public DateTime updated_at;
}

[Serializable]
public class ExecutionGroup
{
  public Execution[] executions;
}
