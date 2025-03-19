<template>
  <SearchPanel title="생산 계획 / 생산 계획 엔진 실행" @search="loadEngineExecuteLog()">
    <template #filter-items>
      <div class="filter-item">
        <q-input outlined dense v-model="startDate" label="시작일">
          <template v-slot:append>
            <q-icon name="event" class="cursor-pointer">
              <q-popup-proxy cover transition-show="fade" transition-hide="fade">
                <q-date v-model="startDate" mask="YYYY-MM-DD" />
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
      </div>
      <div class="filter-item">
        <q-input outlined dense v-model="endDate" label="종료일">
          <template v-slot:append>
            <q-icon name="event" class="cursor-pointer">
              <q-popup-proxy cover transition-show="fade" transition-hide="fade">
                <q-date v-model="endDate" mask="YYYY-MM-DD" />
              </q-popup-proxy>
            </q-icon>
          </template>
        </q-input>
      </div>
      <div class="filter-item"></div>
      <div class="filter-item"></div>
      <div class="filter-item"></div>
      <div class="filter-item"></div>
    </template>
  </SearchPanel>
  <div class="table-container q-mt-md q-px-md">
    <q-table
      class="table"
      :rows="engineExecuteLog"
      :columns="columns"
      selection="single"
      bordered
      flat
      dense
      sticky-header
      virtual-scroll
      :virtual-scroll-slice-size="6"
      :pagination="pagination"
    >
    </q-table>
    <!-- @row-click="onTableRowClick" 
      v-model:selected="selectedTableRows"
     
    -->
  </div>
</template>
<script setup lang="ts">
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import api from 'src/api/axiosInstance'
import { removeZAndParse, formatDateTime, formatDate, addDays } from 'src/utils/dateUtils'
import SearchPanel from 'components/SearchPanel.vue'
import { date, useQuasar } from 'quasar'
import type { EngineExecuteLog } from 'src/types/types'
const $q = useQuasar()
const endDate = ref(formatDate(new Date()))
const startDate = ref(addDays(endDate.value, -7))
const pagination = ref({ rowsPerPage: 0 })

const columns = ref([
  {
    name: 'SIMULATION_VERSION',
    required: true,
    label: '시뮬레이션 버전',
    field: 'SIMULATION_VERSION',
  },
  { name: 'DISPATCH_TYPE', required: true, label: 'Dispatch Type', field: 'DISPATCH_TYPE' },
  {
    name: 'SIMULATION_START_TIME',
    required: true,
    label: '시뮬레이션 시작 시간',
    field: 'SIMULATION_START_TIME',
  },
  {
    name: 'SIMULATION_END_TIME',
    required: true,
    label: '시뮬레이션 종료 시간',
    field: 'SIMULATION_END_TIME',
  },
  { name: 'RUN_USER', required: true, label: '실행 시스템', field: 'RUN_USER' },
  {
    name: 'SIMULATION_EXECUTE_TIME',
    required: true,
    label: '실행 시각',
    field: 'SIMULATION_EXECUTE_TIME',
  },
])

const engineExecuteLog = ref<EngineExecuteLog[]>([])

onMounted(async () => {
  await loadEngineExecuteLog()
})

const loadEngineExecuteLog = async () => {
  try {
    $q.loading.show()
    const response = await api.get('/get-engine-execute-log')
    engineExecuteLog.value = response.data.map((item: any) => ({
      ...item,
      SIMULATION_START_TIME: formatDateTime(removeZAndParse(item.SIMULATION_START_TIME)),
      SIMULATION_END_TIME: formatDateTime(removeZAndParse(item.SIMULATION_END_TIME)),
      SIMULATION_EXECUTE_TIME: formatDateTime(removeZAndParse(item.SIMULATION_EXECUTE_TIME)),
    }))
  } catch (err) {
    console.error('Error fetching data', err)
  } finally {
    $q.loading.hide()
  }
}

// const onTableRowClick = (evt, row) => {
//   selectedTask.value = row
//   selectedTableRows.value = [row]
// }
</script>
<style scoped>
/* 테이블 헤더 스타일 */
:deep(.q-table thead tr) {
  background-color: #f5f5f5;
}

/* 고정 헤더에 그림자 효과 추가 */
:deep(.q-table--sticky-header thead tr:last-child th) {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}
/* 선택된 행 스타일 */
:deep(.q-table tbody tr.selected) {
  background-color: rgba(25, 118, 210, 0.12);
}

/* 체크박스 열 너비를 0으로 설정하여 완전히 숨기기 */
/* .q-table--col-auto-width {
  padding: 0px;
} */
:deep(.q-table--col-auto-width) {
  display: none;
}
:deep(.q-checkbox) {
  display: none;
}

/* 열 사이에 경계선 추가 */
:deep(.q-table th),
:deep(.q-table td) {
  border-right: 1px solid #e0e0e0;
}

/* 마지막 열의 오른쪽 경계선은 제거 (선택 사항) */
:deep(.q-table th:last-child),
:deep(.q-table td:last-child) {
  border-right: none;
}

/* 테두리 스타일을 일관되게 유지 */
:deep(.q-table) {
  border-collapse: collapse;
}
</style>
