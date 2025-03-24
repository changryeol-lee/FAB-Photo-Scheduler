<template>
  <SearchPanel
    title="작업 계획 / 설비 간트 차트"
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
    </template>
  </SearchPanel>

  <!-- 로딩 및 오류 상태 표시 -->
  <!-- <div v-if="loading" class="q-pa-lg flex flex-center">
    <q-spinner size="3em" color="primary" />
    <div class="q-ml-md text-subtitle1">데이터를 불러오는 중...</div>
  </div> 
  -->

  <div v-if="error" class="q-pa-lg flex flex-center text-negative">
    <q-icon name="error" size="2em" />
    <div class="q-ml-md text-subtitle1">{{ error }}</div>
  </div>

  <!-- <div v-else-if="eqpSchedule.length === 0" class="q-pa-lg flex flex-center">
    <q-icon name="info" size="2em" color="info" />
    <div class="q-ml-md text-subtitle1">표시할 데이터가 없습니다.</div>
  </div> -->

  <div v-else>
    <!-- Gantt Chart Container -->
    <div class="gantt-wrapper q-px-md">
      <div class="gantt-wrap">
        <!-- Left Sidebar (Fixed) -->
        <div class="gantt-sidebar">
          <div class="resource-header">설비 ID</div>
          <div class="resource-list" ref="resourceListRef">
            <div v-for="resource in groupedResources" :key="resource.id" class="resource-row">
              {{ resource.id }}
            </div>
          </div>
        </div>

        <div class="gantt-header" ref="ganttHeader">
          <div class="gantt-header-content" ref="ganttHeaderContent">
            <div class="month-row">
              <div
                v-for="month in months"
                :key="month.id"
                class="month-cell"
                :style="{ width: `${month.days.length * columnWidth}px` }"
              >
                {{ month.name }}
              </div>
            </div>
            <div class="day-row">
              <div
                v-for="day in days"
                :key="day.id"
                class="day-cell"
                :style="{ width: `${columnWidth}px` }"
              >
                {{ day.date.getDate() }}
              </div>
            </div>
            <div class="hour-row">
              <div
                v-for="day in days"
                :key="`day-${day.id}`"
                class="day-group"
                :style="{ width: `${columnWidth}px` }"
              >
                <div
                  v-for="hour in hours"
                  :key="`${day.id}-${hour.id}`"
                  class="hour-cell"
                  :style="{ width: `${columnWidth / hoursPerDay}px` }"
                >
                  {{ hour.hour }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="gantt-body" ref="ganttBody">
          <div class="gantt-content" ref="ganttContent">
            <div class="time-grid" ref="timeGrid">
              <div
                v-for="day in days"
                :key="day.id"
                class="day-column"
                :style="{ width: `${columnWidth}px` }"
              >
                <div v-if="isWeekend(day)" class="weekend-overlay"></div>
              </div>
            </div>

            <!-- Task Container -->
            <div class="task-container">
              <div v-for="resource in groupedResources" :key="resource.id" class="resource-tasks">
                <div
                  v-for="task in getTasksForResource(resource.id)"
                  :key="task.SCHEDULE_ID"
                  class="task-item"
                  :class="getTaskClass(task)"
                  :style="getTaskStyle(task)"
                  @click="selectedTask = task"
                >
                  <span v-if="task.WORK_TYPE === 'SETUP'">SETUP</span>
                  <span v-else-if="task.WORK_TYPE === 'OFF'">OFF</span>
                  <span v-else-if="task.WORK_TYPE === 'REWORK'">REWORK</span>
                  <span v-else>{{ task.PRODUCT_ID }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="task-table-container q-mt-md q-px-md" v-if="eqpSchedule.length > 0">
      <q-table
        class="task-detail-table"
        :rows="filteredTasks"
        :columns="columns"
        row-key="SCHEDULE_ID"
        v-model:selected="selectedTableRows"
        @row-click="onTableRowClick"
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
import { date, useQuasar } from 'quasar'
import { removeZAndParse, formatDateTime, formatDate, addDays } from 'src/utils/dateUtils'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import api from '../api/axiosInstance'
import type { EqpSchedule as TaskItem, SelectOption, Equipment } from '../types/types'
import { getLotColor } from 'src/utils/colorUtils'
import SearchPanel from 'components/SearchPanel.vue'
import type { QTableColumn } from 'quasar'

const $q = useQuasar()

// 컴포넌트 최상단에 refs 정의
const ganttHeader = ref(null)
const ganttHeaderContent = ref(null)
const ganttBody = ref(null)
const ganttContent = ref(null)
const timeGrid = ref(null)

const eqpSchedule = ref<TaskItem[]>([])
const resourceListRef = ref(null)
const error = ref<string | null>(null)

// UI 상태 관리
const startDate = ref('') // 2040-03-01
const endDate = ref('') // 2040-03-08
const selectedScheduleVersion = ref(null)
const selectedEqp = ref(null)
const eqps = ref<SelectOption[]>()
const columnWidth = ref(300)
const selectedTask = ref<TaskItem | null>(null)

// 필터링 옵션 생성
const scheduleVersions = ref<any>()

// 컴포넌트 마운트 시 초기화
onMounted(async () => {
  // await loadEqpSchedule()
  await loadEngineExecuteLog()
  await loadEquipment()
  await loadEqpSchedule(selectedScheduleVersion.value)
  ensureHorizontalScroll()
  setupScrollSynchronization()
})

// 시간 간격 (시간)
const timeInterval = ref(4)
const hoursPerDay = computed(() => 24 / timeInterval.value)

// 달력 데이터 생성
const days = computed(() => {
  const start = new Date(startDate.value)
  const end = new Date(endDate.value)
  const daysList = []
  let currentDate = new Date(start)

  while (currentDate <= end) {
    daysList.push({
      id: formatDate(currentDate),
      date: new Date(currentDate),
      isWeekend: currentDate.getDay() === 0 || currentDate.getDay() === 6,
    })
    currentDate.setDate(currentDate.getDate() + 1)
  }
  return daysList
})

const hours = computed(() => {
  const hoursList = []
  for (let i = 0; i < 24; i += timeInterval.value) {
    hoursList.push({
      id: i,
      hour: i < 10 ? `0${i}` : `${i}`,
    })
  }
  return hoursList
})

const months = computed(() => {
  const monthMap = new Map()

  days.value.forEach((day) => {
    const monthKey = date.formatDate(day.date, 'YYYY-MM')
    if (!monthMap.has(monthKey)) {
      monthMap.set(monthKey, {
        id: monthKey,
        name: date.formatDate(day.date, 'M월'),
        days: [],
      })
    }
    monthMap.get(monthKey).days.push(day)
  })

  return Array.from(monthMap.values())
})

// 리소스 그룹화
const groupedResources = computed(() => {
  const uniqueEqpIds = [...new Set(eqpSchedule.value.map((item) => item.EQP_ID))]

  return uniqueEqpIds
    .map((eqpId) => {
      return {
        id: eqpId,
      }
    })
    .sort((a, b) => a.id.localeCompare(b.id))
})

// 주말 여부 확인
const isWeekend = (day: any) => {
  return day.isWeekend
}

// 리소스별 작업 가져오기
const getTasksForResource = (resourceId: any) => {
  const finalDate = new Date(endDate.value)
  finalDate.setHours(0, 0, 0, 0)
  return eqpSchedule.value.filter(
    (task) => task.EQP_ID === resourceId && task.START_TIME < addDays(endDate.value, 1),
  )
}

/* 작업 위치 계산을 위한 수정된 함수 */
const getTaskStyle = (task: TaskItem) => {
  // 시작일로부터의 일수 차이 계산
  const firstDate = new Date(startDate.value)
  firstDate.setHours(0, 0, 0, 0)
  const finalDate = new Date(addDays(endDate.value, 1))
  finalDate.setHours(0, 0, 0, 0)

  const taskStartDate = new Date(task.START_TIME)
  const taskEndDate =
    task.END_TIME > addDays(endDate.value, 1) ? finalDate : new Date(task.END_TIME)

  // 날짜 부분만 가져오기 (시간 정보 제거)
  const startDay = new Date(taskStartDate)
  startDay.setHours(0, 0, 0, 0)

  // 시작 날짜와 간트 차트의 첫 날짜 사이의 일수 차이 계산
  const daysDiff = Math.floor((startDay.getTime() - firstDate.getTime()) / (24 * 60 * 60 * 1000))

  // 해당 날짜 내에서의 시간 위치 계산 (0시부터의 시간)
  const hoursInDay = taskStartDate.getHours() + taskStartDate.getMinutes() / 60

  // 날짜 내 시간 비율 (0-1 사이)
  const hourRatio = hoursInDay / 24

  const durationHours = (taskEndDate.getTime() - taskStartDate.getTime()) / (60 * 60 * 1000)

  // 날짜 내 시간 비율 (0-1 사이)
  const durationRatio = durationHours / 24

  // 위치 및 너비 계산
  const startPos = daysDiff * columnWidth.value + hourRatio * columnWidth.value
  const width = durationRatio * columnWidth.value

  const backgroundColor = getLotColor(task)

  return {
    left: `${startPos}px`,
    width: `${Math.max(width, 20)}px`, // 최소 20px 너비 보장
    backgroundColor,
  }
}

// 작업 클래스 계산
const getTaskClass = (task: TaskItem) => {
  const classes = ['task-item']

  // 선택된 작업 스타일
  if (selectedTask.value && selectedTask.value.SCHEDULE_ID === task.SCHEDULE_ID) {
    classes.push('selected-task')
  }

  return classes.join(' ')
}

const ensureHorizontalScroll = () => {
  void nextTick(() => {
    const totalDaysWidth = days.value.length * columnWidth.value

    // 헤더와 본문 컨텐츠 너비 동일하게 설정
    if (ganttHeaderContent.value) {
      ganttHeaderContent.value.style.width = `${totalDaysWidth}px`
    }

    if (ganttContent.value) {
      ganttContent.value.style.width = `${totalDaysWidth}px`
    }

    if (timeGrid.value) {
      timeGrid.value.style.width = `${totalDaysWidth}px`
    }
  })
}

/* 스크롤 동기화 함수 */
const setupScrollSynchronization = () => {
  if (ganttBody.value && ganttHeader.value) {
    // 본문 스크롤 시 헤더 동기화
    ganttBody.value.addEventListener('scroll', () => {
      requestAnimationFrame(() => {
        if (ganttHeader.value) {
          ganttHeader.value.scrollLeft = ganttBody.value.scrollLeft
        }
      })
    })

    // 헤더 스크롤 시 본문 동기화
    ganttHeader.value.addEventListener('scroll', () => {
      requestAnimationFrame(() => {
        if (ganttBody.value) {
          ganttBody.value.scrollLeft = ganttHeader.value.scrollLeft
        }
      })
    })
  }

  // 설비 목록과 본문의 세로 스크롤 동기화
  if (ganttBody.value && resourceListRef.value) {
    ganttBody.value.addEventListener('scroll', () => {
      requestAnimationFrame(() => {
        if (resourceListRef.value) {
          resourceListRef.value.scrollTop = ganttBody.value.scrollTop
        }
      })
    })

    resourceListRef.value.addEventListener('scroll', () => {
      requestAnimationFrame(() => {
        if (ganttBody.value) {
          ganttBody.value.scrollTop = resourceListRef.value.scrollTop
        }
      })
    })
  }
}

// 테이블 관련 상태
const selectedTableRows = ref([])
const pagination = ref({ rowsPerPage: 0 })
// 선택된 설비의 작업만 필터링
const filteredTasks = computed(() => {
  let tasks = eqpSchedule.value

  if (selectedTask.value?.EQP_ID) {
    tasks = tasks.filter((task) => task.EQP_ID === selectedTask.value.EQP_ID)
  }

  return [...tasks].sort((a, b) => {
    return new Date(a.START_TIME).getTime() - new Date(b.START_TIME).getTime()
  })
})

const columns = ref<QTableColumn[]>([
  { name: 'WORK_TYPE', required: true, label: '작업 유형', field: 'WORK_TYPE' },
  { name: 'EQP_ID', required: true, label: '설비 ID', field: 'EQP_ID' },
  { name: 'PRODUCT_ID', required: true, label: '제품 ID', field: 'PRODUCT_ID' },
  { name: 'LOT_ID', required: true, label: 'LOT ID', field: 'LOT_ID' },
  { name: 'LOT_QTY', required: true, label: 'LOT 수량', field: 'LOT_QTY' },
  { name: 'STEP_ID', required: true, label: '공정 ID', field: 'STEP_ID' },
  { name: 'STEP_NAME', required: true, label: '공정명', field: 'STEP_NAME' },
  { name: 'STEP_SEQ', required: true, label: '공정 순서', field: 'STEP_SEQ' },
  {
    name: 'START_TIME',
    required: true,
    label: '작업 시작 시간',
    field: 'START_TIME',
  },
  { name: 'END_TIME', required: true, label: '작업 종료 시간', field: 'END_TIME' },
])

// 테이블 행 클릭 이벤트
const onTableRowClick = (evt, row) => {
  selectedTask.value = row
  selectedTableRows.value = [row]
}

const loadEngineExecuteLog = async () => {
  try {
    const response = await api.get('/get-engine-execute-log')
    scheduleVersions.value = response.data.map((x) => x.SIMULATION_VERSION)
    selectedScheduleVersion.value = scheduleVersions.value[0]
    startDate.value = formatDate(response.data[0].SIMULATION_START_TIME)
    endDate.value = addDays(formatDate(response.data[0].SIMULATION_END_TIME), -1)
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
  } finally {
    $q.loading.hide()
  }
}
// 간트 차트 선택 시 테이블 연동
watch(selectedTask, (newTask) => {
  if (newTask) {
    selectedTableRows.value = [newTask]
  }
})

// 데이터 변경 시 스크롤 동기화 재설정
watch([days, columnWidth], () => {
  ensureHorizontalScroll()
  setupScrollSynchronization()
})
</script>

<style scoped>
.gantt-wrapper {
  height: 500px;
  margin-bottom: 20px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.05);
  background-color: #fff;
}

.gantt-wrap {
  height: 100%;
  overflow: hidden;
  display: grid;
  grid-template-columns: 200px 1fr;
  grid-template-rows: 60px 1fr;
  position: relative;
}
.gantt-header-content {
  min-width: max-content;
  position: relative;
}

.gantt-sidebar {
  grid-column: 1;
  grid-row: 1 / 3;
  position: sticky;
  left: 0;
  top: 0;
  z-index: 3;
  height: 100%;
  background-color: #f5f7fa;
  border-right: 1px solid #dcdfe6;
  overflow-y: hidden;
  border: 1px solid #dcdfe6;
  display: flex;
  flex-direction: column;
}

.resource-header {
  height: 60px; /* top header height 와 동일하게 설정*/
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #ebeef5;
  border-bottom: 1px solid #dcdfe6;
  font-weight: bold;
  color: #606266;
  flex-shrink: 0; /* 스크롤시 헤더가 줄어들 수 있는 현상 방지 */
}

.resource-list {
  flex-grow: 1;
  overflow-y: auto;
  scrollbar-width: none;
  -ms-overflow-style: none;
}
.resource-list::-webkit-scrollbar {
  display: none;
}
.resource-row {
  height: 50px;
  display: flex;
  align-items: center;
  padding-left: 16px;
  border-bottom: 1px solid #ebeef5;
  color: #606266;
}

.gantt-header {
  grid-column: 2;
  grid-row: 1;
  position: sticky;
  top: 0;
  z-index: 2;
  background-color: #fff;
  overflow-x: auto;
  scrollbar-width: none; /* Firefox */
  -ms-overflow-style: none; /* IE and Edge */
  border: 1px solid #dcdfe6;
}

.gantt-header::-webkit-scrollbar {
  display: none;
}

/* 간트 본문 */
.gantt-body {
  grid-column: 2;
  grid-row: 2;
  overflow: auto; /* 가로/세로 스크롤 허용 */
  position: relative;
  width: 100%;
  border: 1px solid #dcdfe6;
}

.gantt-content {
  position: relative;
  min-width: max-content;
}

.month-row,
.day-row,
.hour-row {
  display: flex;
  height: 20px;
  border-bottom: 1px solid #dcdfe6;
  background-color: #f5f7fa;
}

.month-cell {
  display: flex;
  align-items: center;
  justify-content: center;
  border-right: 1px solid #dcdfe6;
  font-size: 12px;
  background-color: #ebeef5;
  color: #606266;
  font-weight: 600;
}

.day-cell {
  display: flex;
  align-items: center;
  justify-content: center;
  border-right: 1px solid #dcdfe6;
  font-size: 12px;
  color: #606266;
}

.hour-cell {
  display: flex;
  align-items: center;
  justify-content: center;
  border-right: 1px solid #ebeef5;
  font-size: 12px;
  color: #909399;
}

.day-group {
  display: flex;
  border-right: 1px solid #dcdfe6;
  /* flex-wrap: nowrap; 내부 시간 셀이 래핑되지 않도록 */
}

.time-grid {
  display: flex;
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 0;
}

.day-column {
  position: relative;
  float: left;
  height: 100%;
  border-right: 1px solid #ebeef5;
}

.weekend-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.03);
}

.task-container {
  position: relative;
  z-index: 1;
}

.task-item {
  position: absolute;
  height: 80%;
  top: 10%;
  border-radius: 3px;
  font-size: 12px;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
}

.task-item:hover {
  opacity: 0.9;
  z-index: 10;
}

.selected-task {
  /* border: 2px solid #03097c;
  z-index: 5; */
  border: 2.5px solid #7e25f1;
  /* box-shadow: inset 0 0 0 2px #3d5afe; */
}

.resource-tasks {
  position: relative;
  height: 50px; /* Match resource row height */
  border-bottom: 1px solid #ebeef5;
}

.task-table-container {
  height: 220px; /* 테이블 높이 설정 */
  overflow-y: auto;
  position: relative;
}
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

.task-detail-table {
  height: 220px;
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

@media (max-width: 800px) {
  .gantt-wrapper {
    height: 400px;
  }
}

@media (max-width: 1200px) {
  .filter-item {
    flex: 0 0 calc(33.33% - 12px);
  }

  .search-button-container {
    margin-top: 12px;
    width: 100%;
  }

  .search-button {
    width: 100%;
  }
}

@media (max-width: 768px) {
  .filter-item {
    flex: 0 0 calc(50% - 8px);
  }
}

@media (max-width: 576px) {
  .filter-item {
    flex: 0 0 100%;
  }
}
</style>
