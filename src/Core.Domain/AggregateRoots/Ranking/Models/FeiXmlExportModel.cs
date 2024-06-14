/*
 * !!!!!!!!!!!!!!!!!!!!! READ CAREFULLY BEFORE REPLACING WITH NEWLY GENERATED FILE !!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * The following changes are made to this file in order to produce the expected XML
 * - Add DataType = "date" to all DateTime properties - this formats the output date as a simple date string, rather than UTF representation
 * - Change *DateSpecified properties to check if their Date property is not default automatically and return true
 * - Change *SpeedSpecified properties to check if their Speed property is not default automatiicaly and return true
 * - Change RankSpecified property in individual result to check if Status == "R" and Rank is not default
 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 */

using System;
using System.Xml.Serialization;

namespace Core.Domain.AggregateRoots.Ranking.Models;

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.fei.org/Result")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.fei.org/Result", IsNullable = false)]
public partial class HorseSport
{

    private ctGenerated generatedField;

    private ctIssuer issuerField;

    private ctShowResultType eventResultField;

    /// <remarks/>
    public ctGenerated Generated
    {
        get
        {
            return this.generatedField;
        }
        set
        {
            this.generatedField = value;
        }
    }

    /// <remarks/>
    public ctIssuer Issuer
    {
        get
        {
            return this.issuerField;
        }
        set
        {
            this.issuerField = value;
        }
    }

    /// <remarks/>
    public ctShowResultType EventResult
    {
        get
        {
            return this.eventResultField;
        }
        set
        {
            this.eventResultField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctGenerated
{

    private System.DateTime dateField;

    private string softwareField;

    private string softwareVersionField;

    private string organizationField;

    /// <remarks/>
    [XmlIgnore] //Date is added manually after serialization because XmlSerializer does not allow me to format dates differently and here it's required in ISO 8601 format
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime Date
    {
        get
        {
            return this.dateField;
        }
        set
        {
            this.dateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Software
    {
        get
        {
            return this.softwareField;
        }
        set
        {
            this.softwareField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SoftwareVersion
    {
        get
        {
            return this.softwareVersionField;
        }
        set
        {
            this.softwareVersionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Organization
    {
        get
        {
            return this.organizationField;
        }
        set
        {
            this.organizationField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceTotalTeam
{

    private string timeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Time
    {
        get
        {
            return this.timeField;
        }
        set
        {
            this.timeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctPositionTeam
{

    private string statusField;

    private int rankField;

    private bool rankFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Rank
    {
        get
        {
            return this.rankField;
        }
        set
        {
            this.rankField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RankSpecified => Rank != default;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctTeam
{

    private int numberField;

    private bool numberFieldSpecified;

    private string nfField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Number
    {
        get
        {
            return this.numberField;
        }
        set
        {
            this.numberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NumberSpecified
    {
        get
        {
            return this.numberFieldSpecified;
        }
        set
        {
            this.numberFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceTeamResult
{

    private ctTeam teamField;

    private ctPositionTeam positionField;

    private ctPrizeMoney prizeMoneyField;

    private ctEnduranceTotalTeam totalField;

    /// <remarks/>
    public ctTeam Team
    {
        get
        {
            return this.teamField;
        }
        set
        {
            this.teamField = value;
        }
    }

    /// <remarks/>
    public ctPositionTeam Position
    {
        get
        {
            return this.positionField;
        }
        set
        {
            this.positionField = value;
        }
    }

    /// <remarks/>
    public ctPrizeMoney PrizeMoney
    {
        get
        {
            return this.prizeMoneyField;
        }
        set
        {
            this.prizeMoneyField = value;
        }
    }

    /// <remarks/>
    public ctEnduranceTotalTeam Total
    {
        get
        {
            return this.totalField;
        }
        set
        {
            this.totalField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctPrizeMoney
{

    private decimal valueField;

    private string inKindField;

    private decimal inKindValueField;

    private bool inKindValueFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string InKind
    {
        get
        {
            return this.inKindField;
        }
        set
        {
            this.inKindField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal InKindValue
    {
        get
        {
            return this.inKindValueField;
        }
        set
        {
            this.inKindValueField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool InKindValueSpecified
    {
        get
        {
            return this.inKindValueFieldSpecified;
        }
        set
        {
            this.inKindValueFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceTotal
{

    private string timeField;

    private decimal averageSpeedField;

    private bool averageSpeedFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Time
    {
        get
        {
            return this.timeField;
        }
        set
        {
            this.timeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal AverageSpeed
    {
        get
        {
            return this.averageSpeedField;
        }
        set
        {
            this.averageSpeedField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool AverageSpeedSpecified => AverageSpeed != default;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceVetInspection
{

    private stEnduranceVetTypeCode typeField;

    private string eliminationCodeField;

    private int heartRateField;

    private bool heartRateFieldSpecified;

    private string recoveryTimeField;

    private stEnduranceDuringReinspection duringReinspectionField;

    private bool duringReinspectionFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public stEnduranceVetTypeCode Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string EliminationCode
    {
        get
        {
            return this.eliminationCodeField;
        }
        set
        {
            this.eliminationCodeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int HeartRate
    {
        get
        {
            return this.heartRateField;
        }
        set
        {
            this.heartRateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool HeartRateSpecified
    {
        get
        {
            return this.heartRateFieldSpecified;
        }
        set
        {
            this.heartRateFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RecoveryTime
    {
        get
        {
            return this.recoveryTimeField;
        }
        set
        {
            this.recoveryTimeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public stEnduranceDuringReinspection DuringReinspection
    {
        get
        {
            return this.duringReinspectionField;
        }
        set
        {
            this.duringReinspectionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DuringReinspectionSpecified
    {
        get
        {
            return this.duringReinspectionFieldSpecified;
        }
        set
        {
            this.duringReinspectionFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public enum stEnduranceVetTypeCode
{

    /// <remarks/>
    Standard,

    /// <remarks/>
    Final,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public enum stEnduranceDuringReinspection
{

    /// <remarks/>
    no,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("yes-comp")]
    yescomp,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("yes-req")]
    yesreq,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEndurancePhaseResultScore
{

    private decimal phaseAverageSpeedField;

    private bool phaseAverageSpeedFieldSpecified;

    private string phaseTimeField;

    private int rankField;

    private bool rankFieldSpecified;

    private string recoveryTimeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal PhaseAverageSpeed
    {
        get
        {
            return this.phaseAverageSpeedField;
        }
        set
        {
            this.phaseAverageSpeedField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool PhaseAverageSpeedSpecified => PhaseAverageSpeed != default;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string PhaseTime
    {
        get
        {
            return this.phaseTimeField;
        }
        set
        {
            this.phaseTimeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Rank
    {
        get
        {
            return this.rankField;
        }
        set
        {
            this.rankField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RankSpecified => Rank != default;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RecoveryTime
    {
        get
        {
            return this.recoveryTimeField;
        }
        set
        {
            this.recoveryTimeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEndurancePhaseResult
{

    private ctEndurancePhaseResultScore resultField;

    private ctEnduranceVetInspection vetInspectionField;

    private int numberField;

    /// <remarks/>
    public ctEndurancePhaseResultScore Result
    {
        get
        {
            return this.resultField;
        }
        set
        {
            this.resultField = value;
        }
    }

    /// <remarks/>
    public ctEnduranceVetInspection VetInspection
    {
        get
        {
            return this.vetInspectionField;
        }
        set
        {
            this.vetInspectionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Number
    {
        get
        {
            return this.numberField;
        }
        set
        {
            this.numberField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceDayResult
{

    private ctEndurancePhaseResult[] phaseField;

    private int numberField;

    private System.DateTime dateField;

    private bool dateFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Phase")]
    public ctEndurancePhaseResult[] Phase
    {
        get
        {
            return this.phaseField;
        }
        set
        {
            this.phaseField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Number
    {
        get
        {
            return this.numberField;
        }
        set
        {
            this.numberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime Date
    {
        get
        {
            return this.dateField;
        }
        set
        {
            this.dateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DateSpecified => Date != default;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceFirstVetInspection
{

    private stEnduranceFirstVetTypeCode typeField;

    private string eliminationCodeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public stEnduranceFirstVetTypeCode Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string EliminationCode
    {
        get
        {
            return this.eliminationCodeField;
        }
        set
        {
            this.eliminationCodeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public enum stEnduranceFirstVetTypeCode
{

    /// <remarks/>
    First,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctPositionIndiv
{

    private string statusField;

    private int rankField;

    private bool rankFieldSpecified;

    private string complementField;

    private string complementDataField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Rank
    {
        get
        {
            return this.rankField;
        }
        set
        {
            this.rankField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RankSpecified => Rank != default && Status == "R";

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Complement
    {
        get
        {
            return this.complementField;
        }
        set
        {
            this.complementField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ComplementData
    {
        get
        {
            return this.complementDataField;
        }
        set
        {
            this.complementDataField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceComplement
{

    private bool bestConditionField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool BestCondition
    {
        get
        {
            return this.bestConditionField;
        }
        set
        {
            this.bestConditionField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctRefTeam
{

    private string nameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctHorse
{

    private string fEIIDField;

    private string nameField;

    private int headNumberField;

    private bool headNumberFieldSpecified;

    private string nFIDField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int HeadNumber
    {
        get
        {
            return this.headNumberField;
        }
        set
        {
            this.headNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool HeadNumberSpecified
    {
        get
        {
            return this.headNumberFieldSpecified;
        }
        set
        {
            this.headNumberFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceAthlete
{

    private int fEIIDField;

    private string firstNameField;

    private string familyNameField;

    private string competingForField;

    private string nFIDField;

    private int athleteNumberField;

    private bool athleteNumberFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FirstName
    {
        get
        {
            return this.firstNameField;
        }
        set
        {
            this.firstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FamilyName
    {
        get
        {
            return this.familyNameField;
        }
        set
        {
            this.familyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CompetingFor
    {
        get
        {
            return this.competingForField;
        }
        set
        {
            this.competingForField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int AthleteNumber
    {
        get
        {
            return this.athleteNumberField;
        }
        set
        {
            this.athleteNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool AthleteNumberSpecified
    {
        get
        {
            return this.athleteNumberFieldSpecified;
        }
        set
        {
            this.athleteNumberFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceIndivResult
{

    private ctEnduranceAthlete athleteField;

    private ctHorse horseField;

    private ctPrizeMoney prizeMoneyField;

    private ctRefTeam teamField;

    private ctEnduranceComplement complementField;

    private ctPositionIndiv positionField;

    private ctEnduranceFirstVetInspection vetInspectionField;

    private ctEnduranceDayResult[] phasesField;

    private ctEnduranceTotal totalField;

    /// <remarks/>
    public ctEnduranceAthlete Athlete
    {
        get
        {
            return this.athleteField;
        }
        set
        {
            this.athleteField = value;
        }
    }

    /// <remarks/>
    public ctHorse Horse
    {
        get
        {
            return this.horseField;
        }
        set
        {
            this.horseField = value;
        }
    }

    /// <remarks/>
    public ctPrizeMoney PrizeMoney
    {
        get
        {
            return this.prizeMoneyField;
        }
        set
        {
            this.prizeMoneyField = value;
        }
    }

    /// <remarks/>
    public ctRefTeam Team
    {
        get
        {
            return this.teamField;
        }
        set
        {
            this.teamField = value;
        }
    }

    /// <remarks/>
    public ctEnduranceComplement Complement
    {
        get
        {
            return this.complementField;
        }
        set
        {
            this.complementField = value;
        }
    }

    /// <remarks/>
    public ctPositionIndiv Position
    {
        get
        {
            return this.positionField;
        }
        set
        {
            this.positionField = value;
        }
    }

    /// <remarks/>
    public ctEnduranceFirstVetInspection VetInspection
    {
        get
        {
            return this.vetInspectionField;
        }
        set
        {
            this.vetInspectionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Day", IsNullable = false)]
    public ctEnduranceDayResult[] Phases
    {
        get
        {
            return this.phasesField;
        }
        set
        {
            this.phasesField = value;
        }
    }

    /// <remarks/>
    public ctEnduranceTotal Total
    {
        get
        {
            return this.totalField;
        }
        set
        {
            this.totalField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceParticipations
{

    private ctEnduranceIndivResult[] participationField;

    private ctEnduranceTeamResult[] teamParticipationField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Participation")]
    public ctEnduranceIndivResult[] Participation
    {
        get
        {
            return this.participationField;
        }
        set
        {
            this.participationField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("TeamParticipation")]
    public ctEnduranceTeamResult[] TeamParticipation
    {
        get
        {
            return this.teamParticipationField;
        }
        set
        {
            this.teamParticipationField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctPrizeMoneyPlace
{

    private int placeField;

    private bool placeFieldSpecified;

    private decimal amountField;

    private bool amountFieldSpecified;

    private string inKindField;

    private decimal inKindValueField;

    private bool inKindValueFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Place
    {
        get
        {
            return this.placeField;
        }
        set
        {
            this.placeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool PlaceSpecified
    {
        get
        {
            return this.placeFieldSpecified;
        }
        set
        {
            this.placeFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Amount
    {
        get
        {
            return this.amountField;
        }
        set
        {
            this.amountField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool AmountSpecified
    {
        get
        {
            return this.amountFieldSpecified;
        }
        set
        {
            this.amountFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string InKind
    {
        get
        {
            return this.inKindField;
        }
        set
        {
            this.inKindField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal InKindValue
    {
        get
        {
            return this.inKindValueField;
        }
        set
        {
            this.inKindValueField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool InKindValueSpecified
    {
        get
        {
            return this.inKindValueFieldSpecified;
        }
        set
        {
            this.inKindValueFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEndurancePrizeMoneyDetail
{

    private ctPrizeMoneyPlace[] prizeField;

    private string currencyField;

    private decimal totalField;

    private bool totalFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Prize")]
    public ctPrizeMoneyPlace[] Prize
    {
        get
        {
            return this.prizeField;
        }
        set
        {
            this.prizeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Currency
    {
        get
        {
            return this.currencyField;
        }
        set
        {
            this.currencyField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Total
    {
        get
        {
            return this.totalField;
        }
        set
        {
            this.totalField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TotalSpecified
    {
        get
        {
            return this.totalFieldSpecified;
        }
        set
        {
            this.totalFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctPhaseDetailDetail
{

    private int numberOfStarterField;

    private int numberOfFinisherField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int NumberOfStarter
    {
        get
        {
            return this.numberOfStarterField;
        }
        set
        {
            this.numberOfStarterField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int NumberOfFinisher
    {
        get
        {
            return this.numberOfFinisherField;
        }
        set
        {
            this.numberOfFinisherField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctPhaseDetailTime
{

    private System.DateTime startField;

    private System.DateTime endField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime Start
    {
        get
        {
            return this.startField;
        }
        set
        {
            this.startField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime End
    {
        get
        {
            return this.endField;
        }
        set
        {
            this.endField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEndurancePhaseDetailCourse
{

    private int distanceField;

    private bool distanceFieldSpecified;

    private int holdTimeField;

    private bool holdTimeFieldSpecified;

    private bool compulsoryReinspectionField;

    private bool compulsoryReinspectionFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Distance
    {
        get
        {
            return this.distanceField;
        }
        set
        {
            this.distanceField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DistanceSpecified
    {
        get
        {
            return this.distanceFieldSpecified;
        }
        set
        {
            this.distanceFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int HoldTime
    {
        get
        {
            return this.holdTimeField;
        }
        set
        {
            this.holdTimeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool HoldTimeSpecified
    {
        get
        {
            return this.holdTimeFieldSpecified;
        }
        set
        {
            this.holdTimeFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool CompulsoryReinspection
    {
        get
        {
            return this.compulsoryReinspectionField;
        }
        set
        {
            this.compulsoryReinspectionField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool CompulsoryReinspectionSpecified
    {
        get
        {
            return this.compulsoryReinspectionFieldSpecified;
        }
        set
        {
            this.compulsoryReinspectionFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEndurancePhaseDetail
{

    private ctEndurancePhaseDetailCourse courseField;

    private ctPhaseDetailTime executionTimeField;

    private ctPhaseDetailDetail detailField;

    private int numberField;

    private System.DateTime startHourField;

    private bool startHourFieldSpecified;

    /// <remarks/>
    public ctEndurancePhaseDetailCourse Course
    {
        get
        {
            return this.courseField;
        }
        set
        {
            this.courseField = value;
        }
    }

    /// <remarks/>
    public ctPhaseDetailTime ExecutionTime
    {
        get
        {
            return this.executionTimeField;
        }
        set
        {
            this.executionTimeField = value;
        }
    }

    /// <remarks/>
    public ctPhaseDetailDetail Detail
    {
        get
        {
            return this.detailField;
        }
        set
        {
            this.detailField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Number
    {
        get
        {
            return this.numberField;
        }
        set
        {
            this.numberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime StartHour
    {
        get
        {
            return this.startHourField;
        }
        set
        {
            this.startHourField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool StartHourSpecified
    {
        get
        {
            return this.startHourFieldSpecified;
        }
        set
        {
            this.startHourFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceDayDetail
{

    private ctEndurancePhaseDetail[] phaseField;

    private int numberField;

    private System.DateTime dateField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Phase")]
    public ctEndurancePhaseDetail[] Phase
    {
        get
        {
            return this.phaseField;
        }
        set
        {
            this.phaseField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int Number
    {
        get
        {
            return this.numberField;
        }
        set
        {
            this.numberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime Date
    {
        get
        {
            return this.dateField;
        }
        set
        {
            this.dateField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEndurancePhasesDetail
{

    private ctEnduranceDayDetail[] dayField;

    private int totalNumberField;

    private bool totalNumberFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Day")]
    public ctEnduranceDayDetail[] Day
    {
        get
        {
            return this.dayField;
        }
        set
        {
            this.dayField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int TotalNumber
    {
        get
        {
            return this.totalNumberField;
        }
        set
        {
            this.totalNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TotalNumberSpecified
    {
        get
        {
            return this.totalNumberFieldSpecified;
        }
        set
        {
            this.totalNumberFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceDescription
{

    private ctEndurancePhasesDetail phasesField;

    private ctEndurancePrizeMoneyDetail prizesField;

    /// <remarks/>
    public ctEndurancePhasesDetail Phases
    {
        get
        {
            return this.phasesField;
        }
        set
        {
            this.phasesField = value;
        }
    }

    /// <remarks/>
    public ctEndurancePrizeMoneyDetail Prizes
    {
        get
        {
            return this.prizesField;
        }
        set
        {
            this.prizesField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceCompetition
{

    private ctEnduranceDescription descriptionField;

    private ctEnduranceParticipations participationListField;

    private string fEIIDField;

    private string nFIDField;

    private string scheduleCompetitionNrField;

    private string ruleField;

    private string nameField;

    private bool teamField;

    private bool teamFieldSpecified;

    private System.DateTime startDateField;

    private bool startDateFieldSpecified;

    private System.DateTime endDateField;

    private bool endDateFieldSpecified;

    /// <remarks/>
    public ctEnduranceDescription Description
    {
        get
        {
            return this.descriptionField;
        }
        set
        {
            this.descriptionField = value;
        }
    }

    /// <remarks/>
    public ctEnduranceParticipations ParticipationList
    {
        get
        {
            return this.participationListField;
        }
        set
        {
            this.participationListField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ScheduleCompetitionNr
    {
        get
        {
            return this.scheduleCompetitionNrField;
        }
        set
        {
            this.scheduleCompetitionNrField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Rule
    {
        get
        {
            return this.ruleField;
        }
        set
        {
            this.ruleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool Team
    {
        get
        {
            return this.teamField;
        }
        set
        {
            this.teamField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TeamSpecified
    {
        get
        {
            return this.teamFieldSpecified;
        }
        set
        {
            this.teamFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime StartDate
    {
        get
        {
            return this.startDateField;
        }
        set
        {
            this.startDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool StartDateSpecified => StartDate != default;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public System.DateTime EndDate
    {
        get
        {
            return this.endDateField;
        }
        set
        {
            this.endDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool EndDateSpecified => EndDate != default;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctOfficialVeterinarianRole
{

    private int fEIIDField;

    private bool fEIIDFieldSpecified;

    private string nFIDField;

    private string firstNameField;

    private string familyNameField;

    private string nfField;

    private stVeterinarianRole roleField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FEIIDSpecified
    {
        get
        {
            return this.fEIIDFieldSpecified;
        }
        set
        {
            this.fEIIDFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FirstName
    {
        get
        {
            return this.firstNameField;
        }
        set
        {
            this.firstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FamilyName
    {
        get
        {
            return this.familyNameField;
        }
        set
        {
            this.familyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public stVeterinarianRole Role
    {
        get
        {
            return this.roleField;
        }
        set
        {
            this.roleField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public enum stVeterinarianRole
{

    /// <remarks/>
    President,

    /// <remarks/>
    Foreign,

    /// <remarks/>
    Assistant,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctOfficialSteward
{

    private int fEIIDField;

    private bool fEIIDFieldSpecified;

    private string nFIDField;

    private string firstNameField;

    private string familyNameField;

    private string nfField;

    private stOfficialStewardRole roleField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FEIIDSpecified
    {
        get
        {
            return this.fEIIDFieldSpecified;
        }
        set
        {
            this.fEIIDFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FirstName
    {
        get
        {
            return this.firstNameField;
        }
        set
        {
            this.firstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FamilyName
    {
        get
        {
            return this.familyNameField;
        }
        set
        {
            this.familyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public stOfficialStewardRole Role
    {
        get
        {
            return this.roleField;
        }
        set
        {
            this.roleField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public enum stOfficialStewardRole
{

    /// <remarks/>
    Chief,

    /// <remarks/>
    Assistant,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctTechnicalDelegate
{

    private int fEIIDField;

    private bool fEIIDFieldSpecified;

    private string nFIDField;

    private string firstNameField;

    private string familyNameField;

    private string nfField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FEIIDSpecified
    {
        get
        {
            return this.fEIIDFieldSpecified;
        }
        set
        {
            this.fEIIDFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FirstName
    {
        get
        {
            return this.firstNameField;
        }
        set
        {
            this.firstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FamilyName
    {
        get
        {
            return this.familyNameField;
        }
        set
        {
            this.familyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctOfficialJudge
{

    private int fEIIDField;

    private bool fEIIDFieldSpecified;

    private string nFIDField;

    private string firstNameField;

    private string familyNameField;

    private string nfField;

    private stOfficialJudgeRole roleField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FEIIDSpecified
    {
        get
        {
            return this.fEIIDFieldSpecified;
        }
        set
        {
            this.fEIIDFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FirstName
    {
        get
        {
            return this.firstNameField;
        }
        set
        {
            this.firstNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FamilyName
    {
        get
        {
            return this.familyNameField;
        }
        set
        {
            this.familyNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public stOfficialJudgeRole Role
    {
        get
        {
            return this.roleField;
        }
        set
        {
            this.roleField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public enum stOfficialJudgeRole
{

    /// <remarks/>
    President,

    /// <remarks/>
    Foreign,

    /// <remarks/>
    Member,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceOfficial
{

    private ctOfficialJudge[] judgeField;

    private ctTechnicalDelegate[] technicalDelegateField;

    private ctOfficialSteward[] stewardField;

    private ctOfficialVeterinarianRole[] veterinarianField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Judge")]
    public ctOfficialJudge[] Judge
    {
        get
        {
            return this.judgeField;
        }
        set
        {
            this.judgeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("TechnicalDelegate")]
    public ctTechnicalDelegate[] TechnicalDelegate
    {
        get
        {
            return this.technicalDelegateField;
        }
        set
        {
            this.technicalDelegateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Steward")]
    public ctOfficialSteward[] Steward
    {
        get
        {
            return this.stewardField;
        }
        set
        {
            this.stewardField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Veterinarian")]
    public ctOfficialVeterinarianRole[] Veterinarian
    {
        get
        {
            return this.veterinarianField;
        }
        set
        {
            this.veterinarianField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctEnduranceEvent
{

    private ctEnduranceOfficial officialsField;

    private ctEnduranceCompetition[] competitionsField;

    private string fEIIDField;

    private string nFIDField;

    private string codeField;

    private string nfField;

    private string nameField;

    private System.DateTime startDateField;

    private bool startDateFieldSpecified;

    private System.DateTime endDateField;

    private bool endDateFieldSpecified;

    /// <remarks/>
    public ctEnduranceOfficial Officials
    {
        get
        {
            return this.officialsField;
        }
        set
        {
            this.officialsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Competition", IsNullable = false)]
    public ctEnduranceCompetition[] Competitions
    {
        get
        {
            return this.competitionsField;
        }
        set
        {
            this.competitionsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Code
    {
        get
        {
            return this.codeField;
        }
        set
        {
            this.codeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime StartDate
    {
        get
        {
            return this.startDateField;
        }
        set
        {
            this.startDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool StartDateSpecified => StartDate != default;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime EndDate
    {
        get
        {
            return this.endDateField;
        }
        set
        {
            this.endDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool EndDateSpecified => StartDate != default;
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctVenue
{

    private string nameField;

    private string countryField;

    private string fEIIDField;

    private string nFIDField;

    private decimal latitudeField;

    private bool latitudeFieldSpecified;

    private decimal longitudeField;

    private bool longitudeFieldSpecified;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Country
    {
        get
        {
            return this.countryField;
        }
        set
        {
            this.countryField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Latitude
    {
        get
        {
            return this.latitudeField;
        }
        set
        {
            this.latitudeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool LatitudeSpecified
    {
        get
        {
            return this.latitudeFieldSpecified;
        }
        set
        {
            this.latitudeFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Longitude
    {
        get
        {
            return this.longitudeField;
        }
        set
        {
            this.longitudeField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool LongitudeSpecified
    {
        get
        {
            return this.longitudeFieldSpecified;
        }
        set
        {
            this.longitudeFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctShowResult
{

    private ctVenue venueField;

    private ctEnduranceEvent[] enduranceEventField;

    private string fEIIDField;

    private string nFIDField;

    private System.DateTime startDateField;

    private bool startDateFieldSpecified;

    private System.DateTime endDateField;

    private bool endDateFieldSpecified;

    private string nfField;

    /// <remarks/>
    public ctVenue Venue
    {
        get
        {
            return this.venueField;
        }
        set
        {
            this.venueField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("EnduranceEvent")]
    public ctEnduranceEvent[] EnduranceEvent
    {
        get
        {
            return this.enduranceEventField;
        }
        set
        {
            this.enduranceEventField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FEIID
    {
        get
        {
            return this.fEIIDField;
        }
        set
        {
            this.fEIIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NFID
    {
        get
        {
            return this.nFIDField;
        }
        set
        {
            this.nFIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime StartDate
    {
        get
        {
            return this.startDateField;
        }
        set
        {
            this.startDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool StartDateSpecified => StartDate != default;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
    public System.DateTime EndDate
    {
        get
        {
            return this.endDateField;
        }
        set
        {
            this.endDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool EndDateSpecified => StartDate != default;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NF
    {
        get
        {
            return this.nfField;
        }
        set
        {
            this.nfField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctShowResultType
{

    private ctShowResult showField;

    /// <remarks/>
    public ctShowResult Show
    {
        get
        {
            return this.showField;
        }
        set
        {
            this.showField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.fei.org/Result")]
public partial class ctIssuer
{

    private string nameField;

    private string emailField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Email
    {
        get
        {
            return this.emailField;
        }
        set
        {
            this.emailField = value;
        }
    }
}
