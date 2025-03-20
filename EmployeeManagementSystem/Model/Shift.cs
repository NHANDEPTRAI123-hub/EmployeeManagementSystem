using EmployeeManagementSystem;
using System;

public class Shift
{
    private string _shiftName;
    private TimeSpan _startTime;
    private TimeSpan _endTime;

    public Shift(string shiftName, TimeSpan startTime, TimeSpan endTime)
    {
        _shiftName = shiftName;
        _startTime = startTime;
        _endTime = endTime;
    }

    public string ShiftName
    {
        get { return _shiftName; }
        set { _shiftName = value; }
    }

    public TimeSpan StartTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }

    public TimeSpan EndTime
    {
        get { return _endTime; }
        set { _endTime = value; }
    }

    public TimeSpan GetShiftDuration()
    {
        return EndTime - StartTime;
    }

    public bool IsOverlapping(Shift otherShift)
    {
        return (_startTime < otherShift.EndTime && _endTime > otherShift.StartTime);
    }

    public bool IsValidAttendance(Attendance record)
    {
        DateTime checkInTime = record.Timestamp;
        return checkInTime.TimeOfDay >= StartTime && checkInTime.TimeOfDay <= EndTime;
    }
}