/*
 * User: Radish
 * Date: 10/26/2010
 * Time: 7:54 PM
 * Created in SharpDevelop
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PsiComponents
{
    namespace Controls
    {
        public partial class PsiMenuItem
        {   // == Constructors. Nothing much new here.
        
            // In fact everything new is contained within this method.
            private void Initialize()
            {
                isCommandWithDropItems = false;
                customCheckedImage = null;
            }
            
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class.
            /// </summary>
            public PsiMenuItem() : base()
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class that displays the specified text.
            /// </summary>
            /// <param name="text">The text to display on the menu item.</param>
            public PsiMenuItem(string text) : base(text)
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class that displays the specified Image.
            /// </summary>
            /// <param name="image">The Image to display on the control.</param>
            public PsiMenuItem(Image image) : base(image)
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class that displays the specified text and image.
            /// </summary>
            /// <param name="text">The text to display on the menu item.</param>
            /// <param name="image">The Image to display on the control.</param>
            public PsiMenuItem(string text, Image image) : base(text, image)
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class that displays the specified text and image and that does
            /// the specified action when the ToolStripMenuItem is clicked.
            /// </summary>
            /// <param name="text">The text to display on the menu item.</param>
            /// <param name="image">The Image to display on the control.</param>
            /// <param name="onClick">An event handler that raises the Click event when the control is clicked.</param>
            public PsiMenuItem(string text, Image image, EventHandler onClick) : base(text, image, onClick)
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class with the specified name that displays the specified text
            /// and image that does the specified action when the ToolStripMenuItem is clicked. 
            /// </summary>
            /// <param name="text">The text to display on the menu item.</param>
            /// <param name="image">The Image to display on the control.</param>
            /// <param name="onClick">An event handler that raises the Click event when the control is clicked.</param>
            /// <param name="name">The name of the menu item.</param>
            public PsiMenuItem(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class that displays the specified text and image and that
            /// contains the specified ToolStripItem collection. 
            /// </summary>
            /// <param name="text">The text to display on the menu item.</param>
            /// <param name="image">The Image to display on the control.</param>
            /// <param name="dropDownItems">The menu items to display when the control is clicked.</param>
            public PsiMenuItem(string text, Image image, ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
            { this.Initialize(); }
            /// <summary>
            /// Initializes a new instance of the ToolStripMenuItem class that displays the specified text and image, does the
            /// specified action when the ToolStripMenuItem is clicked, and displays the specified shortcut keys.
            /// </summary>
            /// <param name="text">The text to display on the menu item.</param>
            /// <param name="image">The Image to display on the control.</param>
            /// <param name="onClick">An event handler that raises the Click event when the control is clicked.</param>
            /// <param name="shortcutKeys">
            ///   One of the values of Keys that represents the shortcut key for the ToolStripMenuItem.
            /// </param>
            public PsiMenuItem(string text, Image image, EventHandler onClick, Keys shortcutKeys) : base(text, image, onClick, shortcutKeys)
            { this.Initialize(); }        
        }
    }
}
