﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ColecoVisionCartridgeReader.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ColecoVisionCartridgeReader.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Arduino sent an unexpected value. Expected &quot;{0}&quot;, but received &quot;{1}&quot;..
        /// </summary>
        internal static string ArduinoUnexpectedValueMessage {
            get {
                return ResourceManager.GetString("ArduinoUnexpectedValueMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No data was read in from the cartridge. Remove and reinsert the cartridge before trying again..
        /// </summary>
        internal static string BlankCartridge {
            get {
                return ResourceManager.GetString("BlankCartridge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error Reading Cartridge.
        /// </summary>
        internal static string CartridgeReadErrorTitle {
            get {
                return ResourceManager.GetString("CartridgeReadErrorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connecting....
        /// </summary>
        internal static string ConnectingMessage {
            get {
                return ResourceManager.GetString("ConnectingMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The selected file is too large to be a ColecoVision cartridge..
        /// </summary>
        internal static string FileTooLargeMessage {
            get {
                return ResourceManager.GetString("FileTooLargeMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File Read Error.
        /// </summary>
        internal static string FileTooLargeTitle {
            get {
                return ResourceManager.GetString("FileTooLargeTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No cartridge file is currently loaded..
        /// </summary>
        internal static string NoCartridgeFileLoaded {
            get {
                return ResourceManager.GetString("NoCartridgeFileLoaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No cartridge loaded..
        /// </summary>
        internal static string NoCartridgeLoadedMessage {
            get {
                return ResourceManager.GetString("NoCartridgeLoadedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save As Error.
        /// </summary>
        internal static string NoCartridgeLoadedTitle {
            get {
                return ResourceManager.GetString("NoCartridgeLoadedTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are currently no serial ports on this computer.
        ///Verify the Arduino card is plugged into the computer..
        /// </summary>
        internal static string NoSerialPortsMessage {
            get {
                return ResourceManager.GetString("NoSerialPortsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Read Cartridge Error.
        /// </summary>
        internal static string NoSerialPortsTitle {
            get {
                return ResourceManager.GetString("NoSerialPortsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reading Cartridge: {0}% Complete.
        /// </summary>
        internal static string ProgressMessage {
            get {
                return ResourceManager.GetString("ProgressMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Read {0:#,#} bytes from the Arduino, but was expecting {1:#,#} bytes..
        /// </summary>
        internal static string UnexpectedCartridgeSize {
            get {
                return ResourceManager.GetString("UnexpectedCartridgeSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error Occurred.
        /// </summary>
        internal static string UnhandledExceptionTitle {
            get {
                return ResourceManager.GetString("UnhandledExceptionTitle", resourceCulture);
            }
        }
    }
}
