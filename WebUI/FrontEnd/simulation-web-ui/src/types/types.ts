export interface TaskItem {
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

