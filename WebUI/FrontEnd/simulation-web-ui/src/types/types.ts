export interface TaskItem {
  SIMULATION_VERSION: string
  SCHEDULE_ID: string
  EQP_ID: string
  PRODUCT_ID: string
  LOT_ID: string
  LOT_QTY: number
  STEP_ID: string
  START_TIME: Date
  END_TIME: Date
  PROCESS_DURATION: number
  WAIT_DURATION: number
  SETUP_START_TIME: Date
  SETUP_END_TIME: Date
}
