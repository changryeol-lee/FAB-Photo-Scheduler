<template>
  <SearchPanel
    title="생산 계획 / 설비별 생산 계획"
    @search="loadEqpSchedule(selectedScheduleVersion)"
  >
    <template #filter-items>
      <div class="filter-item">
        <q-select
          outlined
          dense
          v-model="selectedScheduleVersion"
          :options="scheduleVersions"
          label="시뮬레이션 버전"
        />
      </div>
      <div class="filter-item">
        <q-select
          outlined
          dense
          v-model="selectedEqp"
          :options="eqps"
          label="설비 ID"
          class="filter-input"
        />
      </div>
      <div class="filter-item"></div>
      <div class="filter-item"></div>

      <div class="filter-item"></div>
      <div class="filter-item"></div>
    </template>
  </SearchPanel>
  <SimTable
    :data="eqpSchedule"
    :columns="columns"
    :offsetHeight="200"
    rowKey="SCHEDULE_ID"
    :tableRowClassFn="getRowClass"
  />
</template>

<script setup lang="ts">
import { useQuasar } from 'quasar'
import { ref, computed, onMounted } from 'vue'
import api from '../../api/axiosInstance'
import SearchPanel from 'components/SearchPanel.vue'
import type { EqpSchedule, Equipment, SelectOption } from 'src/types/types'
import { removeZAndParse, formatDateTime, formatDate, addDays } from 'src/utils/dateUtils'
import SimTable from 'src/components/SimTable.vue'

const $q = useQuasar()
const selectedScheduleVersion = ref(null)
const selectedEqp = ref(null)
const scheduleVersions = ref<string[]>()

const eqpSchedule = ref<EqpSchedule[]>([])
const error = ref<string | null>(null)
const eqps = ref<SelectOption[]>()

const columns = ref([
  { name: 'WORK_TYPE', required: true, label: '작업 유형', field: 'WORK_TYPE' },
  { name: 'EQP_ID', required: true, label: '설비 ID', field: 'EQP_ID' },
  { name: 'PRODUCT_ID', required: true, label: '제품 ID', field: 'PRODUCT_ID' },
  { name: 'LOT_ID', required: true, label: 'LOT ID', field: 'LOT_ID' },
  { name: 'LOT_QTY', required: true, label: 'LOT 수량', field: 'LOT_QTY' },
  { name: 'STEP_ID', required: true, label: '공정 ID', field: 'STEP_ID' },
  { name: 'STEP_NAME', required: true, label: '공정명', field: 'STEP_NAME' },
  { name: 'START_TIME', required: true, label: '작업 시작 시간', field: 'START_TIME' },
  { name: 'END_TIME', required: true, label: '작업 종료 시간', field: 'END_TIME' },
])

const loadEngineExecuteLog = async () => {
  try {
    const response = await api.get('/get-engine-execute-log')
    scheduleVersions.value = response.data.map((x) => x.SIMULATION_VERSION)
    selectedScheduleVersion.value = scheduleVersions.value[0]
  } catch (err) {
    console.error('Error fetching schedule version:', err)
  }
}
const loadEquipment = async () => {
  try {
    const response = await api.get('/get-equipment')
    eqps.value = response.data.map((eqp: Equipment) => ({ label: eqp.EQP_ID, value: eqp.EQP_ID }))
  } catch (err) {
    console.error('Error fetching schedule version:', err)
  }
}

// 데이터 로드 함수
const loadEqpSchedule = async (version?: string): Promise<void> => {
  error.value = null
  $q.loading.show()
  const eqpId: string = selectedEqp.value?.value
  try {
    const response = await api.get('/get-eqp-schedule', {
      params: {
        version: version,
        eqpId: eqpId,
      },
    })
    if (!version) {
      version = response.data.reduce(
        (max, row) => (row.SIMULATION_VERSION > max ? row.SIMULATION_VERSION : max),
        '',
      )
    }
    // API 응답의 날짜 문자열을 Date 객체로 변환
    eqpSchedule.value = response.data
      .filter((item) => item.SIMULATION_VERSION === version)
      .map((item: any) => ({
        ...item,
        START_TIME: formatDateTime(removeZAndParse(item.START_TIME)),
        END_TIME: formatDateTime(removeZAndParse(item.END_TIME)),
      }))
  } catch (err) {
    console.error('Error fetching schedule data:', err)
    error.value = '데이터를 불러오는 중 오류가 발생했습니다.'
  } finally {
    $q.loading.hide()
  }
}

const getRowClass = (row) => {
  // 예: 특정 상태를 가진 행에 클래스 적용
  if (row.WORK_TYPE === 'SETUP') {
    return 'setup-row'
  }
  if (row.WORK_TYPE === 'OFF') {
    return 'off-row'
  }
  if (row.WORK_TYPE === 'REWORK') {
    return 'rework-row'
  }

  return ''
}

onMounted(async () => {
  await loadEngineExecuteLog()
  await loadEquipment()
  await loadEqpSchedule(selectedScheduleVersion.value)
})
</script>
