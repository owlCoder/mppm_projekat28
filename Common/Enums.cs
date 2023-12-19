namespace FTN.Common
{
    public enum PhaseCode : short
    {
        A = 1,
        AB = 2,
        ABC = 3,
        ABCN = 4,
        ABN = 5,
        AC = 6,
        ACN = 7,
        AN = 8,
        B = 9,
        BC = 10,
        BCN = 11,
        BN = 12,
        C = 13,
        CN = 14,
        N = 15,
        s1 = 16,
        s12 = 17,
        s12N = 18,
        s1N = 19,
        s2 = 20,
        s2N = 21,
        Unknown = 0,
    }

    public enum UnitMultiplier : short
    {
       G = 0,
       M = 1,
       T = 2,
       c = 3,
       d = 4,
       k = 5,
       m = 6,
       micro = 7,
       n = 8,
       none = 9,
       p = 10
    }

    public enum UnitSymbol : short
    {
        A = 0,
        F = 1,
        H = 2,
        Hz = 3,
        J = 4,
        N = 5,
        Pa = 6,
        S = 7,
        V = 8,
        VA = 9,
        VAh = 10,
        VAr = 11,
        VArh = 12,
        W = 13,
        Wh = 14,
        deg = 15,
        degC = 16,
        g = 17,
        h = 18,
        m = 19,
        m2 = 20,
        m3 = 21,
        min = 22,
        none = 23,
        ohm = 24,
        rad = 25,
        s = 26
    }

    public enum RegulatingControlModeKind : short
    {
        activePower = 1,
        admittance = 2,
        currentFlow = 3,
        fixed_ = 4,
        powerFactor = 5,
        reactivePower = 6,
        temperature = 7,
        timeScheduled = 8,
        voltage = 9
    }
}
