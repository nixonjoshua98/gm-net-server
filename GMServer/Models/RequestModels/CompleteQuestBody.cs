﻿using GMServer.Models.UserModels;

namespace GMServer.Models.RequestModels
{
    public class CompleteQuestBody
    {
        public int QuestID;
        public DailyUserAccountStats LocalDailyStats;
    }

    public class CompleteMercQuestBody
    {
        public int QuestID;
    }
}
