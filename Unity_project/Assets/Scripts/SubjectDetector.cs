using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class SubjectDetector : MonoBehaviour
{
    UnityEngine.Debug.Log("�����Unity�̃��O�ł�");
    [Serializable]
    public class PeriodTime
    {
        public int Period;          // ��F1, 2, 3...
        public TimeSpan Start;     // �J�n�����i��F08:30�j
        public TimeSpan End;       // �I�������i��F09:20�j
    }

    [Serializable]
    public class PeriodInfo
    {
        public DayOfWeek Day;      // �j���i��FMonday�j
        public int Period;         // �����i��F1, 2, 3...�j
        public string Subject;     // ���ȁi��F���w�j
    }

    // --- �@ �������Ԓ�`�i��j ---
    public List<PeriodTime> periodTimes = new List<PeriodTime>
    {
        new PeriodTime { Period = 1, Start = new TimeSpan(8, 30, 0), End = new TimeSpan(9, 20, 0) },
        new PeriodTime { Period = 2, Start = new TimeSpan(9, 30, 0), End = new TimeSpan(10, 20, 0) },
        new PeriodTime { Period = 3, Start = new TimeSpan(10, 30, 0), End = new TimeSpan(11, 20, 0) },
        new PeriodTime { Period = 4, Start = new TimeSpan(11, 30, 0), End = new TimeSpan(12, 20, 0) },
        new PeriodTime { Period = 5, Start = new TimeSpan(13, 20, 0), End = new TimeSpan(14, 10, 0) },
        new PeriodTime { Period = 6, Start = new TimeSpan(14, 20, 0), End = new TimeSpan(15, 10, 0) }
    };

    // --- �A ���Ԋ��f�[�^�i��j ---
    public List<PeriodInfo> timetable = new List<PeriodInfo>
    {
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 1, Subject = "����" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 2, Subject = "���w" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 3, Subject = "�p��" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 4, Subject = "����" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 5, Subject = "�Љ�" },
        new PeriodInfo { Day = DayOfWeek.Monday, Period = 6, Subject = "�Z�p" },
        // �Ηj�ȍ~�̒�`���ǉ��\
    };

    // --- �B ���C�������F�^�C���X�^���v���狳�Ȃ���� ---
    public string GetSubjectFromTimestamp(DateTime timestamp)
    {
        DayOfWeek day = timestamp.DayOfWeek; // ���j���Ȃ�
        TimeSpan time = timestamp.TimeOfDay; // 10:35�Ȃ�

        // 1. �Y��������T��
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
            return "���Ǝ��ԊO"; // ���Ǝ��ԂɊY�����Ȃ�
        }

        // 2. �j���Ǝ�������Ȗڂ�����
        var match = timetable.FirstOrDefault(p => p.Day == day && p.Period == matchedPeriod);

        return match != null ? match.Subject : "���o�^�i���Ԋ��ɏ��Ȃ��j";
    }

    // --- �C �e�X�g���s�p�i�C�Ӂj ---
    void Start()
    {
        // ��F2025�N6��30���i���j10:35 �� 3���� �� �p��i��� timetable �Q�Ɓj
        DateTime testTime = new DateTime(2025, 6, 30, 10, 35, 0);
        string result = GetSubjectFromTimestamp(testTime);
        Debug.Log("���肳�ꂽ����: " + result); // �� �p��
    }
}
