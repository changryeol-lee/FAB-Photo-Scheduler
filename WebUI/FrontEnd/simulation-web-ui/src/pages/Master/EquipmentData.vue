<template>
  <div class="reference-data-page q-pa-md">
    <div class="text-h5 q-mb-lg">설비</div>
    <div class="row q-col-gutter-md">
      <!-- 첫 번째 행 -->
      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'설비 정보'"
          :data="equipmentData"
          :columns="equipmentColumns"
          :loading="loadingEquipment"
          @refresh="fetchEquipment"
        />
      </div>

      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'설비 제약 정보'"
          :data="eqpArrangeData"
          :columns="eqpArrangeColumns"
          :loading="loadingEqpArrange"
          @refresh="fetchEqpArrange"
        />
      </div>

      <!-- 두 번째 행 -->
      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'유휴 시간'"
          :data="offTimeData"
          :columns="offTimeColumns"
          :loading="loadingOffTime"
          @refresh="fetchOffTimeInfo"
        />
      </div>

      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'설비 SETUP 조건 '"
          :data="setupInfoData"
          :columns="setupInfoColumns"
          :loading="loadingSetupInfo"
          @refresh="fetchSetupInfo"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import MasterTable from '../../components/MasterTable.vue'
import api from '../../api/axiosInstance'
import type { Equipment, EqpArrange, OffTime, SetupInfo } from '../../types/types'
import type { QTableColumn } from 'quasar'

const equipmentData = ref<Equipment[]>([])
const loadingEquipment = ref(false)
const equipmentColumns = ref<QTableColumn[]>([
  { name: 'EQP_ID', align: 'left', label: '설비 ID', field: 'EQP_ID', sortable: true },
  { name: 'EQP_TYPE', align: 'left', label: '설비 유형', field: 'EQP_TYPE', sortable: true },
  { name: 'EQP_STATE', align: 'left', label: '설비 상태', field: 'EQP_STATE', sortable: true },
])

const eqpArrangeData = ref<EqpArrange[]>([])
const loadingEqpArrange = ref(false)
const eqpArrangeColumns = ref<QTableColumn[]>([
  { name: 'EQP_ID', align: 'left', label: '설비 ID', field: 'EQP_ID', sortable: true },
  { name: 'PRODUCT_ID', align: 'left', label: '제품 ID', field: 'PRODUCT_ID', sortable: true },
  { name: 'PROCESS_ID', align: 'left', label: '프로세스 ID', field: 'PROCESS_ID', sortable: true },
  { name: 'STEP_ID', align: 'left', label: '공정 ID', field: 'STEP_ID', sortable: true },
  { name: 'TACT_TIME', align: 'left', label: '공정 시간 (분)', field: 'TACT_TIME', sortable: true },
])

const offTimeData = ref<OffTime[]>([])
const loadingOffTime = ref(false)
const offTimeColumns = ref<QTableColumn[]>([
  { name: 'RULE_TYPE', align: 'left', label: '유휴시간 규칙', field: 'RULE_TYPE', sortable: true },
  { name: 'START_TIME', align: 'left', label: '시작 시각', field: 'START_TIME', sortable: true },
  { name: 'END_TIME', align: 'left', label: '종료 시각', field: 'END_TIME', sortable: true },
  { name: 'DAYS_OF_WEEK', align: 'left', label: '요일', field: 'DAYS_OF_WEEK', sortable: true },
  {
    name: 'START_DATE_TIME',
    align: 'left',
    label: '시작일',
    field: 'START_DATE_TIME',
    sortable: true,
  },
  { name: 'END_DATE_TIME', align: 'left', label: '종료일', field: 'END_DATE_TIME', sortable: true },
])

const setupInfoData = ref<SetupInfo[]>([])
const loadingSetupInfo = ref(false)
const setupInfoColumns = ref<QTableColumn[]>([
  { name: 'EQP_ID', align: 'left', label: '설비 ID', field: 'EQP_ID', sortable: true },
  {
    name: 'SETUP_CONDITION',
    align: 'left',
    label: 'SETUP 조건',
    field: 'SETUP_CONDITION',
    sortable: true,
  },
  {
    name: 'SETUP_TIME',
    align: 'left',
    label: 'SETUP 시간 (분)',
    field: 'SETUP_TIME',
    sortable: true,
  },
])

const fetchEquipment = async () => {
  loadingEquipment.value = true
  try {
    const response = await api.get('/get-equipment')
    equipmentData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingEquipment.value = false
  }
}

const fetchEqpArrange = async () => {
  loadingEqpArrange.value = true
  try {
    const response = await api.get('/get-eqp-arrange')
    eqpArrangeData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingEqpArrange.value = false
  }
}

const fetchOffTimeInfo = async () => {
  loadingOffTime.value = true
  try {
    const response = await api.get('/get-offtime-info')
    offTimeData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingOffTime.value = false
  }
}

const fetchSetupInfo = async () => {
  loadingSetupInfo.value = true
  try {
    const response = await api.get('/get-setup-info')
    setupInfoData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingSetupInfo.value = false
  }
}

onMounted(async () => {
  await Promise.all([fetchEquipment(), fetchEqpArrange(), fetchOffTimeInfo(), fetchSetupInfo()])
})
</script>

<style scoped>
.reference-data-page {
  height: 100%;
}
</style>
