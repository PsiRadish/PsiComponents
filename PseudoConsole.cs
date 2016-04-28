/*
 * Radish Anarcane
 */

using System;
using System.Reflection;
using System.Resources;
using System.Drawing;

namespace PsiComponents
{
    /// <summary>
    /// Separate textbox window with Write method
    /// </summary>
    public class PseudoConsole /*extends*/: System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox ConsoleText;
        
        public PseudoConsole()
        {
            ResourceManager resources = new ResourceManager("PsiRadish.PseudoConsole", Assembly.GetExecutingAssembly());
            this.ConsoleText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            
            // ++ ConsoleText 
            this.ConsoleText.BackColor = System.Drawing.Color.Black;
            this.ConsoleText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleText.Font = new System.Drawing.Font("Fixedsys Excelsior 3.01", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.ConsoleText.ForeColor = System.Drawing.Color.FromArgb(0xC0C0C0);
            this.ConsoleText.Multiline = true;
            this.ConsoleText.Name = "ConsoleText";
            this.ConsoleText.TabIndex = 0;
            
            // == PseudoConsole
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 500);
            
            this.Controls.Add(this.ConsoleText);
            
            this.Icon = (Icon)resources.GetObject("Icon");
            this.Name = "PseudoConsole";
            this.Text = "PseudoConsole";
            this.ResumeLayout(false);
            this.PerformLayout();
            
            this.Visible = false;
        }
        
        public PseudoConsole(string wintitle) : this()
        {
            this.Text = wintitle;
        }
        
        public void Write(string text)
        {
            this.ConsoleText.AppendText(text);
        }
        
        public void WriteLine(string text)
        {
            this.Write(text+Environment.NewLine);
        }
    }
}
