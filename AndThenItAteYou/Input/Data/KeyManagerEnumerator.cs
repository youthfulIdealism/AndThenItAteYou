using Microsoft.Xna.Framework.Input;
using Survive.Input.InputManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive.Input.Data
{
    public static class KeyManagerEnumerator
    {
        public static Dictionary<String, BinaryInputManager> stringAssociations = new Dictionary<string, BinaryInputManager>();
        public static Dictionary<BinaryInputManager, String> reverseAssociations = new Dictionary<BinaryInputManager, string>();

        public static BinaryInputManager num1 = new BinaryKeyManager(Keys.D1);
        public static BinaryInputManager num2 = new BinaryKeyManager(Keys.D2);
        public static BinaryInputManager num3 = new BinaryKeyManager(Keys.D3);
        public static BinaryInputManager num4 = new BinaryKeyManager(Keys.D4);
        public static BinaryInputManager num5 = new BinaryKeyManager(Keys.D5);
        public static BinaryInputManager num6 = new BinaryKeyManager(Keys.D6);
        public static BinaryInputManager num7 = new BinaryKeyManager(Keys.D7);
        public static BinaryInputManager num8 = new BinaryKeyManager(Keys.D8);
        public static BinaryInputManager num9 = new BinaryKeyManager(Keys.D9);
        public static BinaryInputManager num0 = new BinaryKeyManager(Keys.D0);

        public static BinaryInputManager utilTab = new BinaryKeyManager(Keys.Tab);
        public static BinaryInputManager utilLeftShift = new BinaryKeyManager(Keys.LeftShift);
        public static BinaryInputManager utilLeftControl = new BinaryKeyManager(Keys.LeftControl);
        public static BinaryInputManager utilSpace = new BinaryKeyManager(Keys.Space);

        public static BinaryInputManager letrQ = new BinaryKeyManager(Keys.Q);
        public static BinaryInputManager letrW = new BinaryKeyManager(Keys.W);
        public static BinaryInputManager letrE = new BinaryKeyManager(Keys.E);
        public static BinaryInputManager letrR = new BinaryKeyManager(Keys.R);
        public static BinaryInputManager letrT = new BinaryKeyManager(Keys.T);
        public static BinaryInputManager letrY = new BinaryKeyManager(Keys.Y);
        public static BinaryInputManager letrU = new BinaryKeyManager(Keys.U);
        public static BinaryInputManager letrI = new BinaryKeyManager(Keys.I);
        public static BinaryInputManager letrO = new BinaryKeyManager(Keys.O);
        public static BinaryInputManager letrP = new BinaryKeyManager(Keys.P);

        public static BinaryInputManager letrA = new BinaryKeyManager(Keys.A);
        public static BinaryInputManager letrS = new BinaryKeyManager(Keys.S);
        public static BinaryInputManager letrD = new BinaryKeyManager(Keys.D);
        public static BinaryInputManager letrF = new BinaryKeyManager(Keys.F);
        public static BinaryInputManager letrG = new BinaryKeyManager(Keys.G);
        public static BinaryInputManager letrH = new BinaryKeyManager(Keys.H);
        public static BinaryInputManager letrJ = new BinaryKeyManager(Keys.J);
        public static BinaryInputManager letrK = new BinaryKeyManager(Keys.K);
        public static BinaryInputManager letrL = new BinaryKeyManager(Keys.L);

        public static BinaryInputManager letrZ = new BinaryKeyManager(Keys.Z);
        public static BinaryInputManager letrX = new BinaryKeyManager(Keys.X);
        public static BinaryInputManager letrC = new BinaryKeyManager(Keys.C);
        public static BinaryInputManager letrV = new BinaryKeyManager(Keys.V);
        public static BinaryInputManager letrB = new BinaryKeyManager(Keys.B);
        public static BinaryInputManager letrN = new BinaryKeyManager(Keys.N);
        public static BinaryInputManager letrM = new BinaryKeyManager(Keys.M);

        public static BinaryInputManager LMB = new BinaryMouseManager(true);
        public static BinaryInputManager RMB = new BinaryMouseManager(false);

        public static void reBuildStringAssociations()
        {
            stringAssociations.Clear();
           
            addStringAssociation("1", num1);
            addStringAssociation("2", num2);
            addStringAssociation("3", num3);
            addStringAssociation("4", num4);
            addStringAssociation("5", num5);
            addStringAssociation("6", num6);
            addStringAssociation("7", num7);
            addStringAssociation("8", num8);
            addStringAssociation("9", num9);
            addStringAssociation("0", num0);

            addStringAssociation("Tab", utilTab);
            addStringAssociation("Shift_L", utilLeftShift);
            addStringAssociation("Ctrl_L", utilLeftControl);
            addStringAssociation("Space", utilSpace);

            addStringAssociation("Q", letrQ);
            addStringAssociation("W", letrW);
            addStringAssociation("E", letrE);
            addStringAssociation("R", letrR);
            addStringAssociation("T", letrT);
            addStringAssociation("Y", letrY);
            addStringAssociation("U", letrU);
            addStringAssociation("I", letrI);
            addStringAssociation("O", letrO);
            addStringAssociation("P", letrP);

            addStringAssociation("A", letrA);
            addStringAssociation("S", letrS);
            addStringAssociation("D", letrD);
            addStringAssociation("F", letrF);
            addStringAssociation("G", letrG);
            addStringAssociation("H", letrH);
            addStringAssociation("J", letrJ);
            addStringAssociation("K", letrK);
            addStringAssociation("L", letrL);

            addStringAssociation("Z", letrZ);
            addStringAssociation("X", letrX);
            addStringAssociation("C", letrC);
            addStringAssociation("V", letrV);
            addStringAssociation("B", letrB);
            addStringAssociation("N", letrN);
            addStringAssociation("M", letrM);

            addStringAssociation("Mouse 1", LMB);
            addStringAssociation("Mouse 2", RMB);
        }

        private static void addStringAssociation(String str, BinaryInputManager input)
        {
            stringAssociations.Add(str, input);
            reverseAssociations.Add(input, str);
        }
    }
}
