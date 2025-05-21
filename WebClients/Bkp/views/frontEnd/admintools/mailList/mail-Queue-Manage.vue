<template>
<div class="content-wrap">
    <div class="detail-wrap">
        <div class="grid guter-change">
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="CreatedDate">Created Date</label>
                    <common-input placeholder="Created Date" name="CreatedDate"  v-model="Data.CreatedDate" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="FromEmail">From Email</label>
                    <common-input placeholder="From Email" name="FromEmail"  v-model="Data.FromEmail" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="FromName">From Name</label>
                    <common-input placeholder="From Name" name="FromName"  v-model="Data.FromName" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="ToEmail">To Email</label>
                    <common-input placeholder="To Email" name="ToEmail"  v-model="Data.ToEmail" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="Cc">Cc</label>
                    <common-input placeholder="Cc" name="Cc"  v-model="Data.Cc" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="Bcc">Bcc</label>
                    <common-input placeholder="Bcc" name="Bcc"  v-model="Data.Bcc" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="MailSubject">Mail Subject</label>
                    <common-input placeholder="Mail Subject" name="MailSubject"  v-model="Data.MailSubject" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="StatusMsgTry">Send Status</label>
                    <common-input placeholder="Send Status" name="StatusMsgTry"  v-model="Data.StatusMsgTry" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="Retry">Retry</label>
                    <common-input placeholder="Retry" name="Retry"  v-model="Data.Retry" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-4 pb-0">
                <div class="field">
                    <label class="block w-full" for="RetryMsg">Last Retry</label>
                    <common-input placeholder="Last Retry" name="RetryMsg"  v-model="Data.RetryMsg" :disabled="true" ></common-input>
                </div>
            </div>
            <div class="col-12 md:col-12 pb-0">
                <div class="field">
                    <label class="block w-full" for="mailBody">Mail Body</label>
                    <div class="white-box" id="mailBody" v-dompurify-html="Data.MailBody"></div>
                </div>
            </div>
        </div>
        
    </div>
</div>
<div class="p-dialog-footer">
    <common-button btntype="button" :class="'p-button-aux'" label="Cancel" @click="CloseModal" ></common-button>
</div>
</template>
<script setup lang="ts">
    import { inject, ref } from "vue";
    const { ConvertToUserTimeZone } = inject('GlobalFunctions');
    const dialogRef = inject("dialogRef") as any;
    const Data = ref(dialogRef.value.data);
    Data.value.CreatedDate = ConvertToUserTimeZone( Data.value.CreatedDate)
    const CloseModal = () => {
        dialogRef.value.close();
    };
</script>