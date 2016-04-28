/*
 * Author: Kyle Fiegener
 * Date: 10/30/2010
 * Time: 1:53 PM
 * Created in SharpDevelop
 */

// using System;
// using System.Windows.Forms;

// TODO: Install Sandcastle Help File Builder (http://www.ewoodruff.us/shfbdocs/Index.aspx?topic=html/8c0c97d0-c968-4c15-9fe9-e8f3a443c50a.htm)
// and make docs for PsiComponents

namespace PsiComponents
{
    namespace Controls
    {
        /// <summary>
        /// Form with default font set bettererly-like.
        /// </summary>
        public class PsiForm /*extends*/: System.Windows.Forms.Form
        {
            public PsiForm() : base()
            {
                this.Font = System.Drawing.SystemFonts.DialogFont;
                // this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            }
        }
    }
}
