using HumJ.Utilities;
using NUnit.Framework;

namespace NUnitTest
{
    public class MD5Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToHexString()
        {
            Assert.AreEqual("D41D8CD98F00B204E9800998ECF8427E", "".GetMD5Hash().ToHexString());
            Assert.AreEqual("21232F297A57A5A743894A0E4A801FC3", "admin".GetMD5Hash().ToHexString());
            Assert.AreEqual("E10ADC3949BA59ABBE56E057F20F883E", "123456".GetMD5Hash().ToHexString());
            Assert.AreEqual("200820E3227815ED1756A6B531E7E0D2", "qwe123".GetMD5Hash().ToHexString());
            Assert.AreEqual("161EBD7D45089B3446EE4E0D86DBCF92", "P@ssw0rd".GetMD5Hash().ToHexString());
            Assert.AreEqual("F25A2FC72690B780B2A14E140EF6A9E0", "iloveyou".GetMD5Hash().ToHexString());
            Assert.AreEqual("5F4DCC3B5AA765D61D8327DEB882CF99", "password".GetMD5Hash().ToHexString());
            Assert.AreEqual("C44A471BD78CC6C2FEA32B9FE028D30A", "asdfghjkl".GetMD5Hash().ToHexString());
        }
    }
}