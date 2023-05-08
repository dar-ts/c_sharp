using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel2;
using Moq;
using Xunit;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel;

namespace ViewModelTests
{
    public class MainViewModelTests
    {
        [Fact]
        public void ErrorScenario1()
        {
            var er = new Mock<IUIServices>();
            var vd = new ViewData(er.Object);
            //not existing 
            vd.Save("C:\\Users\\luc\\Desktop\\Notexist\\test.txt");
            er.Verify(r => r.ReportError(It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void BasicScenario1()
        {
            var er = new Mock<IUIServices>();
            var vd = new ViewData(er.Object);
            vd.CreateRawData();
            //existing
            vd.Save("C:\\Users\\luc\\Desktop\\test.txt");
            er.Verify(r => r.ReportError(It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public void ErrorScenario2()
        {
            var er = new Mock<IUIServices>();
            var vd = new ViewData(er.Object);
            vd.n_rd = -1;
            vd.ControlsCommand.Execute(null);
            er.Verify(r => r.ReportError(It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public void BasicScenario2()
        {
            var er = new Mock<IUIServices>();
            var vd = new ViewData(er.Object);
            vd.n_rd = 5;
            vd.n_sd = 4;
            vd.leftseg = 6;
            vd.rightseg = 7;
            vd.ControlsCommand.Execute(null);
            er.Verify(r => r.ReportError(It.IsAny<string>()), Times.Never);
        }
        [Fact]
        public void UpdateTriggerTest()
        {
            var er = new Mock<IUIServices>();
            var vd = new ViewData(er.Object);
            var pcr = new PropertyChangedReporter();
            vd.PropertyChanged += pcr.ChangeReport;
            Assert.False(pcr.ChangeHappend);
            vd.ControlsCommand.Execute(null);
            er.Verify(r => r.ReportError(It.IsAny<string>()), Times.Never);
            Assert.True(pcr.ChangeHappend);
        }
        public class PropertyChangedReporter
        {
            public bool ChangeHappend { get; private set; }

            public void ChangeReport(object sender, PropertyChangedEventArgs e) => ChangeHappend = true;
        }

    }
}
