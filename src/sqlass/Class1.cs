// 
//     sqlass
//     Copyright (C) 2011 - 2011 Harald Hoyer
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
//     
//     Comments, bugs and suggestions to hahoyer at yahoo.de

using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.Linq;
using HWClassLibrary.Debug;
using HWClassLibrary.EF.Utility;
using HWClassLibrary.UnitTest;

namespace sqlass
{
    [TestFixture]
    class Class1
    {
        [Test]
        void Test()
        {
            var code = new CodeGenerationTools(this);
            var loader = new MetadataLoader(this);
            var region = new CodeRegion(this, 1);
            var ef = new MetadataTools(this);

            var inputFile = @"Model1.edmx";
            MetadataWorkspace metadataWorkspace = null;
            bool allMetadataLoaded = loader.TryLoadAllMetadata(inputFile, out metadataWorkspace);
            var ItemCollection = (EdmItemCollection)metadataWorkspace.GetItemCollection(DataSpace.CSpace);
            string namespaceName = code.VsNamespaceSuggestion();

            var fileManager = EntityFrameworkTemplateFileManager.Create(this);
        }
    }
}