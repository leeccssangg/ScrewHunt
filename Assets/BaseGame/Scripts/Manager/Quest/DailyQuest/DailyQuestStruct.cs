public class DailyQuest_Login : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.LOGIN) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.LOGIN;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_CollectCash : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.COLLECT_CASH) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.COLLECT_CASH;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_CompleteLevel : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.COMPLETE_LEVEL) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.COMPLETE_LEVEL;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_UseRevive : DailyQuest
{

   public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.USE_REVIVE) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.USE_REVIVE;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_UnlockSlot : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.UNLOCK_SLOT) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.UNLOCK_SLOT;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_UseBooster : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.USE_BOOSTERS) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.USE_BOOSTERS;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}
public class DailyQuest_CompleteCups : DailyQuest
{
    public override void OnNotify(MissionTarget id, string info)
    {
        if (id != MissionTarget.COMPLETE_CUPS) return;
        int amount = int.Parse(info);
        OnCollect(amount);
    }
    public override MissionTarget GetMissionTarget()
    {
        return MissionTarget.COMPLETE_CUPS;
    }
    public override float GetProgress()
    {
        return (float)cl / (float)questConfig.targetAmount;
    }
}


