using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationEngine.BaseEntity
{
    public abstract class Process
    {
        public string ProcessId { get; set; }
        private List<Step> _steps = new List<Step>();

        public void AddStep(Step step)
        {
            _steps.Add(step);
            _steps = _steps.OrderBy(s => s.StepSeq).ToList(); // Step 순서 정렬
        }

        public Step GetNextStep(string currentStepId)
        {
            // 현재 Step 찾기
            var currentStep = _steps.FirstOrDefault(s => s.StepId == currentStepId);
            if (currentStep == null) return null;

            // 현재 StepSeq보다 1 큰 Step 찾기
            return _steps.FirstOrDefault(s => s.StepSeq > currentStep.StepSeq);
        }

        public Step GetPreviousStep(string currentStepId)
        {
            var currentStep = _steps.FirstOrDefault(s => s.StepId == currentStepId);
            if (currentStep == null) return null;

            return _steps.OrderByDescending(s => s.StepSeq)
                         .FirstOrDefault(s => s.StepSeq < currentStep.StepSeq);
        }

        public Step GetFirstStep()
        {
            return _steps.OrderBy(s => s.StepSeq)
                         .FirstOrDefault();
        }
    }
}
