// <copyright file="PexAssemblyInfo.cs" company="Microsoft">Copyright © Microsoft 2010</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "VisualStudioUnitTest")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("TTU.CSC4950.SMC.LogicLayer")]
[assembly: PexInstrumentAssembly("TTU.CSC4950.SMC.DataLayer")]
[assembly: PexInstrumentAssembly("System.Configuration")]
[assembly: PexInstrumentAssembly("TTU.CSC4950.SMC.FrameWork")]
[assembly: PexInstrumentAssembly("PLCSimulator")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "TTU.CSC4950.SMC.DataLayer")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Configuration")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "PLCSimulator")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "TTU.CSC4950.SMC.LogicLayer")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "TTU.CSC4950.SMC.FrameWork")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtStubSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]
[assembly: PexUseStubsFromTestAssembly]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]

