export interface EqpSchedule {
  SIMULATION_VERSION: string
  SCHEDULE_ID: string
  EQP_ID: string
  PRODUCT_ID: string
  LOT_ID: string
  LOT_QTY: number
  STEP_ID: string
  START_TIME: string
  END_TIME: string
  PROCESS_DURATION: number
  WAIT_DURATION: number
  WORK_TYPE: string
}
export interface Product {
  PRODUCT_ID: string
  PROCESS_ID: string
  LOT_SIZE: number
}
export interface Process {
  PROCESS_ID: string
}
export interface Step {
  STEP_ID: string
  STEP_NAME: string
}
export interface StepRoute {
  PROCESS_ID: string
  STEP_ID: string
  STEP_SEQ: number
}
export interface Equipment {
  EQP_ID: string
  EQP_TYPE: string
  EQP_STATE: number
}
export interface EqpArrange {
  PRODUCT_ID: string
  PROCESS_ID: string
  STEP_ID: string
  EQP_ID: string
  TACT_TIME: number
}

export interface OffTime {
  RULE_TYPE: string
  // [Daily / Weekly] 시: 해당 시간을 TimeSpan으로 정의
  START_TIME: string
  END_TIME: string
  // Weekly일 경우, 적용할 요일들
  DAYS_OF_WEEK: string[]
  // Once일 경우, 구체적인 DateTime 범위
  START_DATE_TIME: Date
  END_DATE_TIME: Date
}

export interface SetupInfo {
  EQP_ID: string
  SETUP_CONDITION: string
  SETUP_TIME: number
}

export interface EngineExecuteLog {
  SIMULATION_VERSION: string
  DISPATCH_TYPE: string
  SIMULATION_START_TIME: string
  SIMULATION_END_TIME: string
  RUN_USER: string
  SIMULATION_EXECUTE_TIME: string
}
