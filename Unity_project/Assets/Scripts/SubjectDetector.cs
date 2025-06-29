using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class SubjectDetector : MonoBehaviour
{
    UnityEngine.Debug.Log("これはUnityのログです");
    [Serializable]
    public class PeriodTime
    {
        public int Period;          // 例：1, 2, 3...
        public TimeSpan Start;     // 開始時刻（例：08:30）
        public TimeSpan End;       // 終了時刻（例：09:20）
    }

    [Serializable]
    public class PeriodInfo
    {
        public DayOfWeek Day;      // 曜日（例：Monday）
        public int Period;         // 時限（例：1, 2, 3...）
        public string Subject;     // 教科（例：数学）
    }

    // --- ① 時限時間定義（例） ---
    public List<PeriodTime> periodTimes = new List<PeriodTime>
    {
        new PeriodTime { Period = 1, Start = new TimeSpan(8, 30, 0), End = new TimeSpan(9, 20, 0) },
        new PeriodTime { Period = 2, Start = new TimeSpan(9, 30, 0), End = new TimeSpan(10, 20, 0) },
        new PeriodTime { Period = 3, Start = new TimeSpan(10, 30, 0), End = new TimeSpan(11, 20, 0) },
        new PeriodTime { Period = 4, Start = new TimeSpan(11, 30, 0), End = new TimeSpan(12, 20, 0) },
        new PeriodTime { Period = 5, Start = new TimeSpan(13, 20, 0), End = new TimeSpan(14, 10, 0) },
        new PeriodTime { Period = 6, Start = new TimeSpan(14, 20, 0), End = new TimeSpan(15, 10, 0) }
    };

    // --- ② 時間割データ（例） ---
    public List<PeriodInfo> timetable = new List<PeriodInfo>
    {
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 1, Subject = "国語" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 2, Subject = "数学" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 3, Subject = "英語" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 4, Subject = "理科" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 5, Subject = "社会" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 6, Subject = "技術" },
        // 火曜以降の定義も追加可能
    };

    // --- ③ メイン処理：タイムスタンプから教科を特定 ---
    public string GetSubjectFromTimestamp(DateTime timestamp)
    {
        DayOfWeek day = timestamp.DayOfWeek; // 月曜日など
        TimeSpan time = timestamp.TimeOfDay; // 10:35など

        // 1. 該当時限を探す
        int matchedPeriod = -1;
        foreach (var pt in periodTimes)
        {
            if (time >= pt.Start && time <= pt.End)
            {
                matchedPeriod = pt.Period;
                break;
            }
        }

        if (matchedPeriod == -1)
        {
            return "授業時間外"; // 授業時間に該当しない
        }

        // 2. 曜日と時限から科目を検索
        var match = timetable.FirstOrDefault(p => p.Day == day && p.Period == matchedPeriod);

        return match != null ? match.Subject : "未登録（時間割に情報なし）";
    }

    // --- ④ テスト実行用（任意） ---
    void Start()
    {
        // 例：2025年6月30日（月）10:35 → 3時限 → 英語（上の timetable 参照）
        DateTime testTime = new DateTime(2025, 6, 30, 10, 35, 0);
        string result = GetSubjectFromTimestamp(testTime);
        Debug.Log("判定された教科: " + result); // → 英語
    }
}
