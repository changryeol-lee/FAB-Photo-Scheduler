<template>
  <div class="control-wrapper q-px-md">
    <div class="control-menu-name">기준정보 / BOM 데이터</div>
  </div>
  <div class="reference-data-page q-pa-md">
    <div class="row q-col-gutter-md">
      <!-- 첫 번째 행 -->
      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'제품 정보'"
          :data="productData"
          :columns="productColumns"
          :loading="loadingProduct"
          @refresh="fetchProduct"
        />
      </div>

      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'프로세스 정보'"
          :data="processData"
          :columns="processColumns"
          :loading="loadingProcess"
          @refresh="fetchProcess"
        />
      </div>

      <!-- 두 번째 행 -->
      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'공정 정보'"
          :data="stepData"
          :columns="stepColumns"
          :loading="loadingStep"
          @refresh="fetchStep"
        />
      </div>

      <div class="col-md-6 col-xs-12">
        <MasterTable
          :title="'공정 라우팅 정보'"
          :data="stepRouteData"
          :columns="stepRouteColumns"
          :loading="loadingStepRoute"
          @refresh="fetchStepRoute"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import MasterTable from '../../components/MasterTable.vue'
import api from '../../api/axiosInstance'
import type { Product, Process, Step, StepRoute } from '../../types/types'
import type { QTableColumn } from 'quasar'

const productData = ref<Product[]>([])
const loadingProduct = ref(false)
const productColumns = ref<QTableColumn[]>([
  { name: 'PRODUCT_ID', label: '제품 ID', field: 'PRODUCT_ID', sortable: true },
  { name: 'PROCESS_ID', label: '프로세스 ID', field: 'PROCESS_ID', sortable: true },
  { name: 'LOT_SIZE', label: 'Lot Size', field: 'LOT_SIZE', sortable: true },
])

const processData = ref<Process[]>([])
const loadingProcess = ref(false)
const processColumns = ref<QTableColumn[]>([
  { name: 'PROCESS_ID', label: '프로세스 ID', field: 'PROCESS_ID', sortable: true },
])

const stepData = ref<Step[]>([])
const loadingStep = ref(false)
const stepColumns = ref<QTableColumn[]>([
  { name: 'STEP_ID', label: '공정 ID', field: 'STEP_ID', sortable: true },
  { name: 'STEP_NAME', label: '공정 이름', field: 'STEP_NAME', sortable: true },
])

const stepRouteData = ref<StepRoute[]>([])
const loadingStepRoute = ref(false)
const stepRouteColumns = ref<QTableColumn[]>([
  { name: 'PROCESS_ID', label: '프로세스 ID', field: 'PROCESS_ID', sortable: true },
  { name: 'STEP_ID', label: '공정 ID', field: 'STEP_ID', sortable: true },
  { name: 'STEP_SEQ', label: '공정 순서', field: 'STEP_SEQ', sortable: true },
])

const fetchProduct = async () => {
  loadingProduct.value = true
  try {
    const response = await api.get('/get-product')
    productData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingProduct.value = false
  }
}

const fetchProcess = async () => {
  loadingProcess.value = true
  try {
    const response = await api.get('/get-process')
    processData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingProcess.value = false
  }
}

const fetchStep = async () => {
  loadingStep.value = true
  try {
    const response = await api.get('/get-step')
    stepData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingStep.value = false
  }
}

const fetchStepRoute = async () => {
  loadingStepRoute.value = true
  try {
    const response = await api.get('/get-step-route')
    stepRouteData.value = response.data
  } catch (error) {
    console.error(error)
  } finally {
    loadingStepRoute.value = false
  }
}

onMounted(async () => {
  await Promise.all([fetchProduct(), fetchProcess(), fetchStep(), fetchStepRoute()])
})
</script>

<style scoped>
.reference-data-page {
  height: 100%;
}
</style>
