<template>
  <div class="col-4 md:col-2">
       <Button :label="isHtmlMode ? 'Back to Editor' : 'Edit HTML'" class="p-button-aux" @click="toggleHtmlMode(localValue)" icon="pi pi-code"  />
   </div> 
  
   <div>
      <!--  <Editor  v-if="!isHtmlMode"
           :modelValue="localValue"
           editorStyle="height: 200px"
           @text-change="updateValue($event.htmlValue)"
           />  -->
          <Editor  v-if="!isHtmlMode"
           :modelValue="localValue"
           editorStyle="height: 200px"
           @text-change="updateValue($event.htmlValue)"
           />  
           <textarea
               v-else
               v-model="localValue"
               v-html="localValue"
               style="width: 100%; height: 300px; font-family: monospace; padding: 10px;"
               v-on:input="updateHtmlPreviewValue($event.target.value)"
               ></textarea>  

   
 </div>         
     <div v-if="v$?.value?.[propname ?? name]">
       <div
         class="input-errors" v-for="error in v$.value?.[propname ?? name].$errors" :key="error.$uid">
         <div class="error-msg">{{ error.$message }}</div>
       </div>
     </div>
 </template>
 
 <script setup>
 import { defineProps, defineEmits, ref, watch } from "vue";
 import Editor from "primevue/editor";

 const emit = defineEmits(["update:modelValue"]);
 
 const props = defineProps({
   modelValue: { type: String, default: "" }, // Two-way binding
   IsreadOnly: { type: Boolean, default: false },
   v$: Object, // Validation object
   name: { type: String, required: true },
   propname: { type: String, required: false },
 });


// State

const isHtmlMode = ref(false); // Toggle for HTML mode

// Toolbar configuration
const editorModules = {
 toolbar: {
   container: [
     [{ header: [1, 2, 3, false] }],
     ["bold", "italic", "underline"],
     [{ list: "ordered" }, { list: "bullet" }],
     ["link", "image"],
     [{ color: [] }, { background: [] }],
     ["clean"], // Clear formatting
     [{ html: "Edit HTML" }], // Custom button (functional through toggleHtmlMode)
   ],
 },
};

// Toggle HTML mode
const toggleHtmlMode = (value) => {    
 isHtmlMode.value = !isHtmlMode.value;  
 //updateValue(value);
}
 const localValue = ref(props.modelValue);
  watch(() => props.modelValue, (newValue) => {
       if (newValue !== localValue.value) {
           localValue.value = newValue || ""; // Update localValue
       }
   });

 const updateValue = (value) => {
 if (value !== localValue.value) {
   localValue.value = value;
   emit("update:modelValue", value); // Emit to parent
 }
};

const updateHtmlPreviewValue = (value) => {
 if (value !== undefined) {
   localValue.value = value;
   emit("update:modelValue", value); // Emit to parent
 }
};
 </script>
 <style scoped>
 .editor-container {
   width: 100%;
   height: 100%;
 }
 
 .editor {
   height: 100% !important; /* Ensure it fills the container */
   width: 100% !important; /* Ensure it fills the container */
 }
 
 /* Optional: To control the size of the container itself */
 .editor-container {
   display: flex;
   flex-direction: column;
   height: 100%;
 }
 
 .input-errors {
   margin-top: 10px;
   font-size: 14px;
   color: red;
 }
 
 .error-msg {
   margin: 0;
 }
 </style>