/*
 * Kyle Fiegener
 */

using System;
using System.Windows.Forms;

namespace PsiComponents
{
    namespace Controls
    {
        /// <summary>
        /// Fixes the bottom border on ToolStrips using the System renderer.
        /// http://connect.microsoft.com/VisualStudio/feedback/details/92862/toolstrip-always-draws-a-border-on-the-bottom-with-rendermode-system-and-docked-left-or-right
        /// </summary>
        public class PsiStripSystemRenderer /*extends*/: System.Windows.Forms.ToolStripSystemRenderer
        {
            public PsiStripSystemRenderer() : base()
            { }
            
            protected override void OnRenderToolStripBorder(System.Windows.Forms.ToolStripRenderEventArgs e)
            {  // don't call base
                //base.OnRenderToolStripBorder(e);
            }
        }
        
        /// <summary>
        /// Removes the normal blueish rounded line at the bottom and right edges when using this renderer.
        /// </summary>
        public class PsiStripProfessionalRenderer : System.Windows.Forms.ToolStripProfessionalRenderer
        {
            public PsiStripProfessionalRenderer() : base()
            { }
            
            protected override void OnRenderToolStripBorder(System.Windows.Forms.ToolStripRenderEventArgs e)
            {  // don't call base
                //base.OnRenderToolStripBorder(e);
            }
        }
        
        /// <summary>
        /// ToolStrip that uses the Psi renderers by default.
        /// </summary>
        public class PsiStrip /*extends*/: System.Windows.Forms.ToolStrip
        {
            /// <summary>
            /// Initializes a new instance of the PsiStrip class.
            /// </summary>
            public PsiStrip() : base()
            { }
            
            /// <summary>
            /// Initializes a new instance of the PsiStrip class with the specified array of ToolStripItems.
            /// </summary>
            /// <param name="items">An array of ToolStripItem objects.</param>
            public PsiStrip(params ToolStripItem[] items) : base(items)
            { }
            
            /// <summary>
            /// Automatically replaces default System and Professional renderers with Psi versions.
            /// </summary>
            protected override void OnRendererChanged(EventArgs e)
            {
                // if this has a ToolStrip*Renderer and not a PsiStrip*Renderer...
                if (this.Renderer is ToolStripSystemRenderer && !(this.Renderer is PsiStripSystemRenderer))
                    this.Renderer = new PsiStripSystemRenderer();
                else if (this.Renderer is ToolStripProfessionalRenderer && !(this.Renderer is PsiStripProfessionalRenderer))
                    this.Renderer = new PsiStripProfessionalRenderer();
                
                base.OnRendererChanged(e);
            }
        }
    }
}
