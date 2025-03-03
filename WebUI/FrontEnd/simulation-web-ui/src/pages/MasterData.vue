<template>
  <q-page class="q-pa-md">
    <q-card>
      <q-card-section>
        <div class="text-h5">기준정보 관리</div>
      </q-card-section>

      <q-separator />

      <q-card-section>
        <q-btn color="primary" label="제품 데이터 불러오기" @click="loadProducts" />
        <q-table
          v-if="products.length > 0"
          :rows="products"
          :columns="columns"
          row-key="product_id"
          dense
          bordered
          class="q-mt-md"
        />
      </q-card-section>
      <q-card-section>
        <q-btn color="primary" label="proc 데이터 불러오기" @click="loadProcess" />
        <q-table
          v-if="processes.length > 0"
          :rows="processes"
          :columns="proc_columns"
          row-key="process_id"
          dense
          bordered
          class="q-mt-md"
        />
      </q-card-section>
      <q-card-section>
        <q-btn color="primary" label="공정 데이터 불러오기" @click="loadStep" />
        <q-table
          v-if="steps.length > 0"
          :rows="steps"
          :columns="step_columns"
          row-key="step_id"
          dense
          bordered
          class="q-mt-md"
        />
      </q-card-section>
      <q-card-section>
        <q-btn color="primary" label="라우팅 데이터 불러오기" @click="loadStepRoute" />
        <q-table
          v-if="step_route.length > 0"
          :rows="step_route"
          :columns="step_route_columns"
          dense
          bordered
          class="q-mt-md"
        />
      </q-card-section>
      <q-card-section>
        <q-btn color="primary" label="설비 데이터 불러오기" @click="loadEquipment" />
        <q-table
          v-if="equipment.length > 0"
          :rows="equipment"
          :columns="equipment_columns"
          dense
          bordered
          class="q-mt-md"
        />
      </q-card-section>
      <q-card-section>
        <q-btn color="primary" label="설비별 제약 조건" @click="loadEqpArrange" />
        <q-table
          v-if="eqp_arrange.length > 0"
          :rows="eqp_arrange"
          :columns="eqp_arrange_columns"
          dense
          bordered
          class="q-mt-md"
        />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import api from '../api/axiosInstance'

const products = ref<Array<{ product_id: string; product_name: string }>>([])
const columns = ref([
  { name: 'PRODUCT_ID', required: true, label: '제품 ID', field: 'PRODUCT_ID' }, //align: 'left',
  { name: 'PROCESS_ID', required: true, label: '프로세스 ID', field: 'PROCESS_ID' },
  { name: 'LOT_SIZE', required: true, label: 'LOT SIZE', field: 'LOT_SIZE' },
])

const processes = ref<Array<{ process_id: string }>>([])
const proc_columns = ref([
  { name: 'PROCESS_ID', required: true, label: '프로세스 ID', field: 'PROCESS_ID' },
])

const steps = ref<Array<{ step_id: string; step_name: string }>>([])
const step_columns = ref([
  { name: 'STEP_ID', required: true, label: '공정 ID', field: 'STEP_ID' },
  { name: 'STEP_NAME', required: true, label: '공정 이름', field: 'STEP_NAME' },
])

const step_route = ref<Array<{ process_id: string; step_id: string; step_seq: string }>>([])
const step_route_columns = ref([
  { name: 'PROCESS_ID', required: true, label: '프로세스 ID', field: 'PROCESS_ID' },
  { name: 'STEP_ID', required: true, label: '공정 ID', field: 'STEP_ID' },
  { name: 'STEP_SEQ', required: true, label: '공정 순서', field: 'STEP_SEQ' },
])

const equipment = ref<Array<{ eqp_id: string; eqp_state: string }>>([])
const equipment_columns = ref([
  { name: 'EQP_ID', required: true, label: '설비 ID', field: 'EQP_ID' },
  { name: 'EQP_STATE', required: true, label: '설비 상태', field: 'EQP_STATE' },
])

const eqp_arrange = ref<
  Array<{
    product_id: string
    process_id: string
    step_id: string
    eqp_id: string
    tact_time: number
  }>
>([])
const eqp_arrange_columns = ref([
  { name: 'PRODUCT_ID', required: true, label: '제품 ID', field: 'PRODUCT_ID' },
  { name: 'PROCESS_ID', required: true, label: 'PROC ID', field: 'PROCESS_ID' },
  { name: 'STEP_ID', required: true, label: '공정 ID', field: 'STEP_ID' },
  { name: 'EQP_ID', required: true, label: '설비 ID', field: 'EQP_ID' },
  { name: 'TACT_TIME', required: true, label: '공정 시간(초)', field: 'TACT_TIME' },
])

const loadProducts = async (): Promise<any> => {
  try {
    const response = await api.get('/get-product')
    products.value = response.data
  } catch (error) {
    console.error('Error fetching data:', error)
    throw error
  }
}
const loadProcess = async (): Promise<any> => {
  try {
    const response = await api.get('/get-process')
    processes.value = response.data
  } catch (error) {
    console.error('Error fetching data:', error)
    throw error
  }
}

const loadStep = async (): Promise<any> => {
  try {
    const response = await api.get('/get-step')
    steps.value = response.data
  } catch (error) {
    console.error('Error fetching data:', error)
    throw error
  }
}
const loadStepRoute = async (): Promise<any> => {
  try {
    const response = await api.get('/get-step-route')
    step_route.value = response.data
  } catch (error) {
    console.error('Error fetching data:', error)
    throw error
  }
}

const loadEquipment = async (): Promise<any> => {
  try {
    const response = await api.get('/get-equipment')
    equipment.value = response.data
  } catch (error) {
    console.error('Error fetching data:', error)
    throw error
  }
}

const loadEqpArrange = async (): Promise<any> => {
  try {
    const response = await api.get('/get-eqp-arrange')
    eqp_arrange.value = response.data
  } catch (error) {
    console.error('Error fetching data:', error)
    throw error
  }
}

// const loadProducts = async () => {
//   try {
//     const getProductURL =
//       'https://oebokqkcd36v4qa5yfieqys3ny0qjrjr.lambda-url.ap-northeast-2.on.aws/'
//     const response = await axios.get(getProductURL)
//     products.value = response.data
//   } catch (error) {
//     console.error('제품 데이터 호출 중 오류 발생:', error)
//     throw error
//   }
// }

const masterData = ref<{ id: number; name: string }[]>([])

const fetchMasterData = () => {
  masterData.value = [
    { id: 1, name: '설비 목록' },
    { id: 2, name: '제품 정보' },
    { id: 3, name: '공정 단계' },
  ]
}
</script>
