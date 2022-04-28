﻿using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Gateways.Desktop.Controls;
using EnduranceJudge.Gateways.Desktop.Controls.Ranking;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class RanklistPrinter : PrintTemplate
{
    public RanklistPrinter(string competitionName, RanklistAggregate rankList)
        : base(competitionName)
    {
        var controls = RanklistControl.CreateResultControls(rankList);
        foreach (var control in controls)
        {
            control.Measure(this.PrintDimension.PageSize);
            control.Arrange(new Rect());
            control.Scale(0.75);
            this.AddPrintContent(control);
        }
    }
}
