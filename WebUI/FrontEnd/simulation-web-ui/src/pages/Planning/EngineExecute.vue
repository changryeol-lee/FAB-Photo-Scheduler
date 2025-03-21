<template>
  <div class="control-container">
    <div class="control-wrapper q-px-md">
      <div class="control-menu-name">생산 계획 / 생산 계획 엔진 실행</div>

      <div class="header-container q-pa-md">
        <!-- 좌우 분리 레이아웃 -->
        <div class="row filter-container">
          <!-- 왼쪽: 이력 조회 부분 -->
          <div class="col-12 filter-wrapper">
            <div class="filter-row filter-first-row">
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
            </div>
            <div class="filter-row">
              <div class="filter-item"></div>
              <div class="filter-item"></div>
              <div class="filter-item">
                <q-btn
                  color="primary"
                  icon-right="search"
                  label="이력 조회"
                  no-caps
                  class="search-button"
                  @click="onSearch()"
                />
              </div>
            </div>
          </div>
          <!-- 오른쪽: 엔진 실행 부분 -->
          <div class="col-12 filter-wrapper">
            <div class="filter-row filter-first-row">
              <div class="filter-item">
                <q-input outlined dense v-model="simulationStartDate" label="시뮬레이션 시작일">
                  <template v-slot:append>
                    <q-icon name="event" class="cursor-pointer">
                      <q-popup-proxy cover transition-show="fade" transition-hide="fade">
                        <q-date v-model="simulationStartDate" mask="YYYY-MM-DD" />
                      </q-popup-proxy>
                    </q-icon>
                  </template>
                </q-input>
              </div>
              <div class="filter-item">
                <q-input
                  outlined
                  dense
                  v-model="simulationPeriod"
                  label="시뮬레이션 기간 (일)"
                  :disable="true"
                >
                </q-input>
              </div>
              <div class="filter-item">
                <q-select
                  outlined
                  dense
                  v-model="dispatchType"
                  :options="dispatchTypes"
                  label="디스패치 유형"
                />
              </div>
            </div>
            <div class="filter-row">
              <div class="filter-item"></div>
              <div class="filter-item"></div>
              <div class="filter-item">
                <q-btn
                  color="secondary"
                  icon-right="play_arrow"
                  label="엔진 실행"
                  no-caps
                  class="search-button"
                  @click="onClickRunEngine()"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="q-mt-md q-px-md">
      <q-table
        class="double-search-table"
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
    </div>
  </div>
</template>
<script setup lang="ts">
import api, { engineApi } from 'src/api/axiosInstance'
import { onMounted, ref } from 'vue'
import { useQuasar } from 'quasar'
import type { EngineExecuteLog } from 'src/types/types'
import { addDays, formatDate, formatDateTime, removeZAndParse } from 'src/utils/dateUtils'
const $q = useQuasar()
const endDate = ref(formatDate(new Date()))
const startDate = ref(addDays(endDate.value, -7))
const pagination = ref({ rowsPerPage: 0 })

const simulationStartDate = ref(formatDate(new Date()))
const simulationPeriod = ref(7)
const dispatchType = ref('FIFO')
const dispatchTypes = ref(['FIFO', 'MIN_SETUP'])

const columns = ref([
  {
    name: 'SIMULATION_VERSION',
    required: true,
    label: '시뮬레이션 버전',
    field: 'SIMULATION_VERSION',
  },
  { name: 'DISPATCH_TYPE', required: true, label: '디스패치 타입', field: 'DISPATCH_TYPE' },
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

const onSearch = async () => {
  await loadEngineExecuteLog()
}

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

const onClickRunEngine = () => {
  $q.dialog({
    title: '엔진 실행 확인',
    message: '생산 계획 엔진을 실행하시겠습니까?',
    persistent: true,
    ok: {
      push: true,
    },
    cancel: {
      push: true,
      color: 'negative',
    },
  }).onOk(() => {
    void runEngine()
  })
}

const runEngine = async () => {
  $q.loading.show({ message: '엔진 실행 중...' })
  try {
    const response = await engineApi.post('/EngineExecute/execute-engine', {
      DispatchType: dispatchType.value,
      RunUser: 'WEB USER',
      SimulationStartTime: simulationStartDate.value,
      SimulationPeriod: simulationPeriod.value,
    })

    $q.notify({
      color: 'positive',
      message: '엔진이 성공적으로 실행되었습니다.',
      icon: 'check_circle',
    })
  } catch (error) {
    console.error('엔진 실행 오류:', error)
    $q.notify({
      color: 'negative',
      message: '엔진 실행 중 오류가 발생했습니다.',
      caption: error.response?.data?.message || '서버 통신 중 오류가 발생했습니다.',
      icon: 'error',
    })
  } finally {
    await loadEngineExecuteLog()
    $q.loading.hide()
  }
}
</script>
<style scoped>
/* 테이블 헤더 고정 CSS */
:deep(.q-table) thead {
  position: sticky;
  top: 0;
  z-index: 1;
}

:deep(.q-table) thead tr th {
  position: sticky;
  top: 0;
  z-index: 1;
  background-color: #f5f5f5;
}

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

.filter-container {
  position: relative;
  gap: 32px;
}

.filter-first-row {
  margin-bottom: 5px;
}

/* 각 div의 오른쪽에 구분선 추가 */
.filter-wrapper::after {
  content: '';
  position: absolute;
  right: -18px; /* gap의 절반만큼 + 선의 두께 */
  top: 50%;
  transform: translateY(-50%);
  height: 100%;
  width: 2px; /* 선의 두께 */
  background-color: #ccc;
}

/* 마지막 요소에는 구분선 제거 */
.filter-wrapper:last-child::after {
  display: none;
}

.vertical-separator {
  width: 2px;
  background-color: #dcdfe6;
  transform: translateX(-50%);
}

.filter-wrapper {
  height: auto;
  width: calc(50% - 16px);
  position: relative;
}
.filter-row {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
}

.filter-item {
  flex: 1;
  min-width: 180px;
}

/* 반응형 디자인 */
@media (max-width: 991.98px) {
  .filter-item {
    flex: 0 0 calc(50% - 8px);
  }
}

@media (max-width: 576px) {
  .filter-item {
    flex: 0 0 100%;
  }
}
.double-search-table {
  height: calc(100vh - 240px); /* 화면 전체 높이에서 상단/하단 요소 크기 빼기 */
  max-height: calc(100vh - 240px);
  overflow-y: auto; /* 필요할 경우 내부 스크롤 허용 */
}
</style>
